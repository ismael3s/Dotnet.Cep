using System.Net.Http.Json;
using Dotnet.Cep.Core.Interfaces;

namespace Dotnet.Cep.Infra.Implementations.CepsGateways;

file sealed record ApiCepResponse(string State, string City, string Address, string District);

public sealed class ApiCepGateway(HttpClient client) : ICepGateway
{
    public async Task<CepResponse> Find(string cep, CancellationToken cancellationToken = default)
    {
        var formattedCep = cep.Insert(5, "-");
        var httpResponse = await client.GetAsync($"/file/apicep/{formattedCep}.json", cancellationToken: cancellationToken);
        httpResponse.EnsureSuccessStatusCode();
        var response = await httpResponse.Content.ReadFromJsonAsync<ApiCepResponse>(cancellationToken: cancellationToken);
        return new CepResponse(cep, response?.State, response?.District, response?.Address, response?.City, "ApiCep");
    }
}