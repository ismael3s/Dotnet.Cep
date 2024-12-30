using Dotnet.Cep.Core.Interfaces;
using Dotnet.Cep.Core.Services;
using Dotnet.Cep.Infra.Implementations.CepsGateways;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Dotnet.Cep.Infra;

public static class InfraExtension
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
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
        services.AddSingleton<ICepGateway, ViaCepGateway>();
        services.AddSingleton<ICepGateway, BrasilApiCepGateway>();
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
                LocalCacheExpiration = TimeSpan.FromSeconds(10)
            };
        });
#pragma warning restore EXTEXP0018
    }
}