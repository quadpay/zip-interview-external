using System.ComponentModel.DataAnnotations;

namespace Zip.Promotions.Exercise.Configuration;

public sealed class CacheOptions
{
    public const string SectionName = "Cache";

    [Range(typeof(TimeSpan), "00:00:01", "01:00:00")]
    public TimeSpan PromotionTtl { get; init; } = TimeSpan.FromMinutes(5);
}
