using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Newtonsoft.Json;
using TixFactory.BusTracker.Api.Entities;
using TixFactory.MongoDB;
using TixFactory.Operations;
using CollectionExtensions = TixFactory.MongoDB.CollectionExtensions;

namespace TixFactory.BusTracker.Api;

/// <summary>
/// Updates all the known bus operators in the database.
/// </summary>
public class UpdateBusOperatorsOperation : IAsyncAction
{
    private readonly IMongoCollection<BusOperatorEntity> _BusOperators;
    private readonly HttpClient _HttpClient;
    private readonly IConfiguration _Configuration;
    private readonly string _Region;

    public UpdateBusOperatorsOperation(IMongoCollection<BusOperatorEntity> busOperators, HttpClient httpClient, IConfiguration configuration)
    {
        _BusOperators = busOperators ?? throw new ArgumentNullException(nameof(busOperators));
        _HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _Region = _HttpClient.BaseAddress?.Host;
    }

    public async Task<OperationError> ExecuteAsync(CancellationToken cancellationToken)
    {
        var existingBusOperatorsQuery = await _BusOperators.FindAsync(Builders<BusOperatorEntity>.Filter.Empty, cancellationToken: cancellationToken);
        var existingBusOperators = await existingBusOperatorsQuery.ToListAsync(cancellationToken);
        var validTransitOperators = await LoadTransitOperatorsAsync(cancellationToken);
        var operatorIds = new HashSet<string>();

        foreach (var transitOperator in validTransitOperators)
        {
            if (transitOperator.PrimaryMode != "bus")
            {
                // This app only cares about bus operators.
                continue;
            }

            operatorIds.Add(transitOperator.Id);

            var record = existingBusOperators.FirstOrDefault(o => o.Region == _Region && o.OperatorId == transitOperator.Id);
            if (record != null)
            {
                await _BusOperators.UpdateOneAsync(e => e.Id == record.Id, entity =>
                {
                    if (entity.Enabled
                        && entity.Name == transitOperator.Name
                        && entity.ShortName == transitOperator.ShortName
                        && entity.Monitored == transitOperator.Monitored
                        && entity.Region == _Region)
                    {
                        // Record already matches expected state, do nothing.
                        return false;
                    }

                    entity.Enabled = true;
                    entity.Name = transitOperator.Name;
                    entity.ShortName = transitOperator.ShortName;
                    entity.Monitored = transitOperator.Monitored;
                    entity.Region = _HttpClient.BaseAddress?.Host;
                    entity.Updated = DateTime.UtcNow;
                    return true;
                }, cancellationToken);
            }
            else
            {
                await _BusOperators.InsertOneAsync(new BusOperatorEntity
                {
                    OperatorId = transitOperator.Id,
                    Name = transitOperator.Name,
                    ShortName = transitOperator.ShortName,
                    Monitored = transitOperator.Monitored,
                    Region = _Region,
                    Enabled = true,
                    Updated = DateTime.UtcNow,
                    Created = DateTime.UtcNow
                }, new InsertOneOptions
                {
                    Comment = CollectionExtensions.WriteComment
                }, cancellationToken);
            }
        }

        foreach (var busOperator in existingBusOperators)
        {
            if (busOperator.Region != _Region)
            {
                // This bus operator belongs to a different region, don't touch it.
                continue;
            }

            if (operatorIds.Contains(busOperator.OperatorId))
            {
                // This bus operator is still valid
                continue;
            }

            if (busOperator.Enabled)
            {
                await _BusOperators.UpdateOneAsync(e => e.Id == busOperator.Id, e =>
                {
                    e.Enabled = false;
                    return true;
                }, cancellationToken);
            }
        }

        return null;
    }

    private async Task<IReadOnlyCollection<TransitOperatorResult>> LoadTransitOperatorsAsync(CancellationToken cancellationToken)
    {
        var apiKey = _Configuration.GetValue<string>("511_API_KEY");
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new ApplicationException("Missing 511_API_KEY, cannot load bus operators");
        }

        var response = await _HttpClient.GetStringAsync(new Uri($"http://api.511.org/transit/operators?apiKey={apiKey}"), cancellationToken);
        return JsonConvert.DeserializeObject<TransitOperatorResult[]>(response);
    }
}
