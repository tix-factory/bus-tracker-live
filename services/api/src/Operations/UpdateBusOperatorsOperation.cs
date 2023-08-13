using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
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
    private readonly ITransitClient _TransitClient;

    /// <summary>
    /// Initializes a new <seealso cref="UpdateBusOperatorsOperation"/>.
    /// </summary>
    /// <param name="busOperators">The <seealso cref="IMongoCollection{TDocument}"/> for <seealso cref="BusOperatorEntity"/>.</param>
    /// <param name="transitClient">The <seealso cref="ITransitClient"/>.</param>
    /// <exception cref="ArgumentNullException">
    /// - <paramref name="busOperators"/>
    /// - <paramref name="transitClient"/>
    /// </exception>
    public UpdateBusOperatorsOperation(IMongoCollection<BusOperatorEntity> busOperators, ITransitClient transitClient)
    {
        _BusOperators = busOperators ?? throw new ArgumentNullException(nameof(busOperators));
        _TransitClient = transitClient ?? throw new ArgumentNullException(nameof(transitClient));
    }

    /// <inheritdoc cref="IAsyncAction.ExecuteAsync"/>
    public async Task<OperationError> ExecuteAsync(CancellationToken cancellationToken)
    {
        var existingBusOperatorsQuery = await _BusOperators.FindAsync(Builders<BusOperatorEntity>.Filter.Empty, cancellationToken: cancellationToken);
        var existingBusOperators = await existingBusOperatorsQuery.ToListAsync(cancellationToken);
        var validTransitOperators = await _TransitClient.LoadTransitOperatorsAsync(cancellationToken);
        var operatorIds = new HashSet<string>();

        foreach (var transitOperator in validTransitOperators)
        {
            if (transitOperator.PrimaryMode != "bus")
            {
                // This app only cares about bus operators.
                continue;
            }

            operatorIds.Add(transitOperator.Id);

            var record = existingBusOperators.FirstOrDefault(o => o.Region == transitOperator.Region && o.OperatorId == transitOperator.Id);
            if (record != null)
            {
                await _BusOperators.UpdateOneAsync(e => e.Id == record.Id, entity =>
                {
                    // Sometimes the short name doesn't exist with the public transit authority, so don't overwrite it to null.
                    var shortName = transitOperator.ShortName ?? entity.ShortName;

                    if (entity.Enabled
                        && entity.Name == transitOperator.Name
                        && entity.ShortName == shortName
                        && entity.Monitored == transitOperator.Monitored
                        && entity.Region == transitOperator.Region)
                    {
                        // Record already matches expected state, do nothing.
                        return false;
                    }

                    entity.Enabled = true;
                    entity.Name = transitOperator.Name;
                    entity.ShortName = shortName;
                    entity.Monitored = transitOperator.Monitored;
                    entity.Region = transitOperator.Region;
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
                    Region = transitOperator.Region,
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
}
