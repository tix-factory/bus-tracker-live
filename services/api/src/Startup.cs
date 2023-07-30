using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
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
    public void Configure(IApplicationBuilder app)
    {
        UseConfiguration(app);
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
    }
}
