using Dotnet.Cep.Core.Interfaces;
using Microsoft.Extensions.Caching.Hybrid;

namespace Dotnet.Cep.Core.Services;

public sealed class CepService(IEnumerable<ICepGateway> cepsGateway, HybridCache cache)
{
    public async Task<CepResponse> GetCepAsync(string cep, CancellationToken cancellationToken = default)
    {
        return await cache.GetOrCreateAsync(cep, async token =>
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
            var linkedToken = cts.Token;
            var tasks = cepsGateway.Select(cepGateway => cepGateway.Find(cep, linkedToken));
            var result = await Task.WhenAny(tasks);
            await cts.CancelAsync();
            return await result;
        }, cancellationToken: cancellationToken);
    }
}