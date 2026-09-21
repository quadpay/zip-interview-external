namespace Zip.Promotions.Exercise.Configuration;

public sealed record PromotionTierConfiguration
{
    public decimal MinimumOrderAmount { get; init; }

    public decimal DiscountAmount { get; init; }
}
