using Zip.Promotions.Exercise.Promotions;

namespace Zip.Promotions.Exercise.Eligibility;

public sealed record PromotionEvaluation(Promotion Promotion, bool IsEligible, decimal DiscountAmount, string? Reason)
{
    public static PromotionEvaluation Eligible(Promotion promotion, decimal discountAmount) =>
        new(promotion, true, discountAmount, null);

    public static PromotionEvaluation Ineligible(Promotion promotion, string reason) =>
        new(promotion, false, 0m, reason);
}
