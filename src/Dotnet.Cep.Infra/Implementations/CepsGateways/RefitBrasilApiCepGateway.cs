using Dotnet.Cep.Core.Interfaces;
using Refit;

namespace Dotnet.Cep.Infra.Implementations.CepsGateways;

public interface IRefitBrazilApiCepGateway
{
    [Get("/api/cep/v2/{cep}")]
    public Task<CepResponse> Find(string cep, CancellationToken cancellationToken = default);
}

public sealed class BrasilApiCepGateway(IRefitBrazilApiCepGateway httpClient) : ICepGateway
{
    public async Task<CepResponse> Find(string cep, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.Find(cep, cancellationToken);
        return response with
        {
            Gateway = "BrasilApi"
        };
    }
}