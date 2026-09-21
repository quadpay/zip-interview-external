using Zip.Promotions.Exercise.Promotions;

namespace Zip.Promotions.Exercise.Configuration;

public sealed record PromotionConfiguration
{
    public Guid Id { get; init; }

    public string? Name { get; init; }

    public Guid? MerchantId { get; init; }

    public PromotionAudience Audience { get; init; }

    public DateTimeOffset StartsAt { get; init; }

    public DateTimeOffset EndsAt { get; init; }

    public PromotionStatus Status { get; init; }

    public IReadOnlyList<PromotionTierConfiguration> Tiers { get; init; } = [];

    public Promotion ToPromotion() => new()
    {
        Id = this.Id,
        Name = this.Name ?? string.Empty,
        MerchantId = this.MerchantId,
        Audience = this.Audience,
        StartsAt = this.StartsAt,
        EndsAt = this.EndsAt,
        Status = this.Status,
        Tiers = [.. this.Tiers.Select(tier => new PromotionTier(tier.MinimumOrderAmount, tier.DiscountAmount))],
    };
}
