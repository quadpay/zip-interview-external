using System.ComponentModel.DataAnnotations;

namespace Zip.Promotions.Exercise.Configuration;

public sealed class CustomerAuthenticationOptions
{
    public const string SectionName = "Authentication";

    [Required]
    [MinLength(1)]
    public string CustomerHeaderName { get; init; } = "X-Customer-Id";
}
