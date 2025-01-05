using Dotnet.Cep.Core.Interfaces;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace Dotnet.Cep.Core.Services;

public sealed class CepService(IEnumerable<ICepGateway> cepsGateway, HybridCache cache)
{
    public async Task<CepResponse> GetCepAsync(string cep, bool mustCache = true,
        CancellationToken cancellationToken = default)
    {
        if (mustCache)
        {
            return await cache.GetOrCreateAsync(cep, async token => await Do(cep, token),
                cancellationToken: cancellationToken);
        }
        return await Do(cep, cancellationToken);
    }

    private async Task<CepResponse> Do(string cep, CancellationToken token = default)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
        var linkedToken = cts.Token;
        var tasks = cepsGateway.Select(cepGateway => cepGateway.Find(cep, linkedToken));
        var result = await Task.WhenAny(tasks);
        await cts.CancelAsync();
        return await result;
    }
}