namespace Zip.Promotions.Exercise.Contracts;

/// <summary>
/// An order a customer is about to place.
/// </summary>
public sealed record AssessPromotionRequest
{
    /// <summary>The draft order being quoted, which exists upstream before it is confirmed.</summary>
    /// <example>order-1001</example>
    public string OrderId { get; init; } = string.Empty;

    /// <example>b2000000-0000-0000-0000-000000000001</example>
    public Guid MerchantId { get; init; }

    /// <example>175.00</example>
    public decimal OrderAmount { get; init; }
}
