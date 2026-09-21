using Microsoft.Extensions.Options;

using Zip.Promotions.Exercise.Configuration;
using Zip.Promotions.Exercise.Persistence;
using Zip.Promotions.Exercise.Promotions;

namespace Zip.Promotions.Exercise.Caching;

public sealed class CachedPromotionRepository : IPromotionRepository
{
    public const string CacheKey = "promotions:active";

    private readonly IPromotionRepository inner;
    private readonly ICache cache;
    private readonly IOptionsMonitor<CacheOptions> options;

    public CachedPromotionRepository(IPromotionRepository inner, ICache cache, IOptionsMonitor<CacheOptions> options)
    {
        this.inner = inner;
        this.cache = cache;
        this.options = options;
    }

    public async Task<IReadOnlyList<Promotion>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        var cached = await this.cache.GetAsync<Promotion[]>(CacheKey, cancellationToken).ConfigureAwait(false);

        if (cached is not null)
        {
            return cached;
        }

        var promotions = (await this.inner.GetActiveAsync(cancellationToken).ConfigureAwait(false)).ToArray();

        await this.cache
            .SetAsync(CacheKey, promotions, this.options.CurrentValue.PromotionTtl, cancellationToken)
            .ConfigureAwait(false);

        return promotions;
    }
}
