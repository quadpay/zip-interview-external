namespace Zip.Promotions.Exercise.Configuration;

public sealed class SeedOptions
{
    public const string SectionName = "Seed";

    public IReadOnlyList<CustomerOrderHistorySeed> CustomerOrderHistory { get; init; } = [];

}
