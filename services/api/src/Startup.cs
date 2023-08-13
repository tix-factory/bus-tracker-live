using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using TixFactory.BusTracker.Api.Entities;
using TixFactory.MongoDB;

namespace TixFactory.BusTracker.Api;

/// <inheritdoc cref="Http.Service.Startup"/>
public class Startup : Http.Service.Startup
{
    /// <summary>
    /// Startup method for the application, after <see cref="ConfigureServices"/>.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
    /// <param name="busOperators">The collection for bus operators.</param>
    public void Configure(IApplicationBuilder app, IMongoCollection<BusOperatorEntity> busOperators)
    {
        UseConfiguration(app);

        // Create database indices
        CreateIndices(busOperators);
    }

    /// <inheritdoc cref="Http.Service.Startup.ConfigureServices"/>
    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

        // Database Dependencies
        services.AddMongoCollection<BusOperatorEntity>();
        services.AddMongoCollection<BusStopEntity>();
        services.AddMongoCollection<BusLineEntity>();
        services.AddMongoCollection<BusScheduleEntity>();
        services.AddMongoCollection<BusTripEntity>();
        services.AddMongoCollection<BusTripStopEntity>();

        // Main Dependencies
        services.AddHttpClient<ITransitClient, TransitClient>(httpClient =>
        {
            httpClient.BaseAddress = new Uri("https://api.511.org");
        }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip
        });

        // Operations
        services.AddSingleton<UpdateBusOperatorsOperation>();
        services.AddSingleton<RefreshBusScheduleOperation>();
    }

    private void CreateIndices(IMongoCollection<BusOperatorEntity> busOperators)
    {
        var operatorIndexKeys = Builders<BusOperatorEntity>.IndexKeys.Ascending(e => e.Region).Ascending(e => e.OperatorId);
        busOperators.Indexes.CreateOne(new CreateIndexModel<BusOperatorEntity>(operatorIndexKeys, new CreateIndexOptions
        {
            Unique = true
        }));
    }
}
