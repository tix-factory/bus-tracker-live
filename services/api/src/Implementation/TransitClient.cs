using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace TixFactory.BusTracker.Api;

/// <inheritdoc cref="ITransitClient"/>
public class TransitClient : ITransitClient
{
    private readonly IConfiguration _Configuration;
    private readonly HttpClient _HttpClient;
    private readonly string _APIKey;

    /// <summary>
    /// The API key to send requests to 511 with
    /// </summary>
    private string ApiKey
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_APIKey))
            {
                throw new ApplicationException("Missing 511_API_KEY");
            }

            return _APIKey;
        }
    }

    public TransitClient(IConfiguration configuration, HttpClient httpClient)
    {
        _Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _APIKey = GetApiKey();
    }

    /// <inheritdoc cref="ITransitClient.LoadTransitOperatorsAsync"/>
    public async Task<IReadOnlyCollection<TransitOperatorResult>> LoadTransitOperatorsAsync(CancellationToken cancellationToken)
    {
        var response = await _HttpClient.GetStringAsync($"transit/operators?api_key={ApiKey}", cancellationToken);
        var result = JsonConvert.DeserializeObject<TransitOperatorResult[]>(response);

        foreach (var r in result)
        {
            r.Region = _HttpClient.BaseAddress?.Host;
        }

        return result;
    }

    /// <inheritdoc cref="ITransitClient.LoadBusStopsAsync"/>
    public async Task<IReadOnlyCollection<ScheduledStopPointResult>> LoadBusStopsAsync(string operatorId, CancellationToken cancellationToken)
    {
        //var apiKey = GetApiKey();
        var apiKey = ApiKey;
        var response = await _HttpClient.GetStringAsync($"transit/stops?api_key={apiKey}&operator_id={operatorId}", cancellationToken);
        var result = JsonConvert.DeserializeObject<ExternalContentResult>(response);
        return result.Data.Data.Data;
    }

    private string GetApiKey(string operatorId = "DEFAULT")
    {
        var operators = _Configuration.GetSection("511");
        var operatorConfiguration = operators.GetSection(operatorId);
        return operatorConfiguration.GetValue<string>("API_KEY");
    }
}
