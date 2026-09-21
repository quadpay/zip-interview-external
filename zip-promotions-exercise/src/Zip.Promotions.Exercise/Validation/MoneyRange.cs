namespace Zip.Promotions.Exercise.Validation;

/// <summary>
/// Bounds every monetary field on the request contracts is held to, so the validators cannot drift apart.
/// </summary>
internal static class MoneyRange
{
    /// <summary>An order has to be worth something before it is worth discounting.</summary>
    public const decimal SmallestOrderAmount = 0.01m;

    /// <summary>A tier may start at zero, which is how a promotion with no threshold is expressed.</summary>
    public const decimal SmallestTierAmount = 0m;

    public const decimal LargestAmount = 1_000_000m;
}
