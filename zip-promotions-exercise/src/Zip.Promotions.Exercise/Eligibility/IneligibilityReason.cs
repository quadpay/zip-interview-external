namespace Zip.Promotions.Exercise.Eligibility;

public static class IneligibilityReason
{
    public const string OutsideDateRange = "promotion.outside_date_range";
    public const string MerchantMismatch = "promotion.merchant_mismatch";
    public const string AudienceMismatch = "promotion.audience_mismatch";
    public const string BelowMinimumOrderAmount = "promotion.below_minimum_order_amount";
}
