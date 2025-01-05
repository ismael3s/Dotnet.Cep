using Dotnet.Cep.Core.Interfaces;
using Dotnet.Cep.Core.Services;
using Dotnet.Cep.Infra.Implementations.CepsGateways;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Refit;

namespace Dotnet.Cep.Infra;

public static class InfraExtension
{
    
    public static IHostApplicationBuilder ConfigureOpenTelemetry(this IHostApplicationBuilder builder)
    {
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });
        builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation();
            })
            .WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();
            }).UseOtlpExporter();
        return builder;
    }

    
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureHttpClientDefaults(http =>
        {
            http.AddStandardResilienceHandler();
        });
        AddHttpClient(services, configuration);
        AddServices(services);
        AddCache(services);
        return services;
    }


    private static void AddHttpClient(IServiceCollection services, IConfiguration configuration)
    {
        services.AddRefitClient<IRefitBrazilApiCepGateway>()
            .ConfigureHttpClient(c => { c.BaseAddress = new Uri(configuration["BrasilApi:Url"]!); });
        services.AddRefitClient<IRefitViaCepGateway>()
            .ConfigureHttpClient(c => { c.BaseAddress = new Uri(configuration["ViaCep:Url"]!); });
        services.AddHttpClient<ICepGateway, ApiCepGateway>(client =>
        {
            client.BaseAddress = new Uri(configuration["ApiCep:Url"]!);
        });
        services.AddScoped<ICepGateway, ViaCepGateway>();
        services.AddScoped<ICepGateway, BrasilApiCepGateway>();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddSingleton<CepService>();
    }

    private static void AddCache(this IServiceCollection services)
    {
#pragma warning disable EXTEXP0018
        services.AddHybridCache(options =>
        {
            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                LocalCacheExpiration = TimeSpan.FromSeconds(5)
            };
        });
#pragma warning restore EXTEXP0018
    }
}