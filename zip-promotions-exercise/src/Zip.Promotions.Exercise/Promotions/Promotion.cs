namespace Zip.Promotions.Exercise.Promotions;

public sealed class Promotion
{
    private readonly PromotionTier[] tiers = [];

    public required Guid Id { get; init; }

    public required string Name { get; init; }

    /// <summary>The merchant this promotion is scoped to, or null when it is offered across every merchant.</summary>
    public Guid? MerchantId { get; init; }

    public PromotionAudience Audience { get; init; }

    public DateTimeOffset StartsAt { get; init; }

    public DateTimeOffset EndsAt { get; init; }

    public PromotionStatus Status { get; init; }

    public required IReadOnlyList<PromotionTier> Tiers
    {
        get => this.tiers;
        init => this.tiers = [.. value.OrderByDescending(tier => tier.MinimumOrderAmount)];
    }

    public bool IsActive => this.Status is PromotionStatus.Active;

    public PromotionTier? ResolveTier(decimal orderAmount) =>
        Array.Find(this.tiers, tier => orderAmount >= tier.MinimumOrderAmount);

    public decimal CalculateDiscount(decimal orderAmount) =>
        Money.Round(Math.Clamp(this.ResolveTier(orderAmount)?.DiscountAmount ?? 0m, 0m, Math.Max(orderAmount, 0m)));

    public bool IsWithinWindow(DateTimeOffset instant) => instant >= this.StartsAt && instant < this.EndsAt;
}
