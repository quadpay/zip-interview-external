using Zip.Promotions.Exercise.Customers;

namespace Zip.Promotions.Exercise.Eligibility;

public sealed record PromotionEligibilityContext(
    Guid CustomerId,
    Guid MerchantId,
    decimal OrderAmount,
    DateTimeOffset EvaluatedAt,
    CustomerOrderHistorySummary History);
