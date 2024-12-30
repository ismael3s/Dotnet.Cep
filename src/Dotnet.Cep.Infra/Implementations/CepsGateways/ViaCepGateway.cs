using Dotnet.Cep.Core.Interfaces;
using Refit;

namespace Dotnet.Cep.Infra.Implementations.CepsGateways;

public interface IRefitViaCepGateway
{
    [Get("/ws/{cep}/json")]
    Task<ViaCepResponse> Find(string cep);
}

public sealed record ViaCepResponse(string Logradouro, string Bairro, string Localidade, string Estado);

public sealed class ViaCepGateway(IRefitViaCepGateway httpClient) : ICepGateway
{
    public async Task<CepResponse> Find(string cep, CancellationToken ct = default)
    {
        var response = await httpClient.Find(cep);
        return new CepResponse(cep, response.Estado, response.Bairro, City: response.Localidade,
            Street: response.Logradouro, Gateway: "ViaCep");
    }
}