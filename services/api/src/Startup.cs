using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using TixFactory.Operations;

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
    }
}
