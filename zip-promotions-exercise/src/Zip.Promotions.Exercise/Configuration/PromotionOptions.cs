namespace Zip.Promotions.Exercise.Configuration;

public sealed class PromotionOptions
{
    public const string SectionName = "Promotions";

    public IReadOnlyList<PromotionConfiguration> Items { get; init; } = [];
}
