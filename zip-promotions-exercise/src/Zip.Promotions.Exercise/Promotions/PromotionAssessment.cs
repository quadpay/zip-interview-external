namespace Zip.Promotions.Exercise.Promotions;

/// <summary>
/// Result of a promotion assessment.
/// </summary>
public sealed record PromotionAssessment(
    bool PromotionAvailable,
    Guid? PromotionId,
    string? PromotionName,
    decimal DiscountAmount,
    decimal OrderAmount)
{
    public static PromotionAssessment None(decimal orderAmount) => new(false, null, null, 0m, orderAmount);
}
