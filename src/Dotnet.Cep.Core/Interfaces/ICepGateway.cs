namespace Dotnet.Cep.Core.Interfaces;

public sealed record CepResponse(
    string Cep,
    string State,
    string Neighborhood,
    string Street,
    string City,
    string Gateway);

public interface ICepGateway
{
    public Task<CepResponse> Find(string cep, CancellationToken cancellationToken = default);
}