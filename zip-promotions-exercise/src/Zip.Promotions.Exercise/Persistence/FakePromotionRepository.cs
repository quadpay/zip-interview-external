using Microsoft.Extensions.Options;

using Zip.Promotions.Exercise.Configuration;
using Zip.Promotions.Exercise.Promotions;

namespace Zip.Promotions.Exercise.Persistence;

/// <summary>
/// Stands in for the promotions table. Rows are materialised from the <c>Promotions</c> configuration section at
/// startup and never change.
/// </summary>
public sealed class FakePromotionRepository : IPromotionRepository
{
    private readonly IReadOnlyList<Promotion> rows;

    public FakePromotionRepository(IOptions<PromotionOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options);

        this.rows = [.. options.Value.Items.Select(item => item.ToPromotion())];
    }

    public Task<IReadOnlyList<Promotion>> GetActiveAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<Promotion>>(
        [
            .. this.rows
                .Where(promotion => promotion.IsActive)
                .OrderBy(promotion => promotion.Id),
        ]);
}
