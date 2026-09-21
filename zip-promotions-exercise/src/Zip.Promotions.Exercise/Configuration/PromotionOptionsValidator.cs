using Microsoft.Extensions.Options;

namespace Zip.Promotions.Exercise.Configuration;

public sealed class PromotionOptionsValidator : IValidateOptions<PromotionOptions>
{
    public ValidateOptionsResult Validate(string? name, PromotionOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var failures = new List<string>();
        var seenIds = new HashSet<Guid>();

        for (var index = 0; index < options.Items.Count; index++)
        {
            Validate(options.Items[index], index, seenIds, failures);
        }

        return failures.Count == 0
            ? ValidateOptionsResult.Success
            : ValidateOptionsResult.Fail(failures);
    }

    private static void Validate(
        PromotionConfiguration promotion,
        int index,
        HashSet<Guid> seenIds,
        List<string> failures)
    {
        var label = promotion.Id == Guid.Empty ? $"Promotions:Items:{index}" : $"Promotion '{promotion.Id}'";

        if (promotion.Id == Guid.Empty)
        {
            failures.Add($"{label}: Id is required.");
        }
        else if (!seenIds.Add(promotion.Id))
        {
            failures.Add($"{label}: Id is duplicated.");
        }

        if (string.IsNullOrWhiteSpace(promotion.Name))
        {
            failures.Add($"{label}: Name is required.");
        }

        if (promotion.EndsAt <= promotion.StartsAt)
        {
            failures.Add($"{label}: EndsAt must be after StartsAt.");
        }

        ValidateTiers(promotion, label, failures);
    }

    private static void ValidateTiers(PromotionConfiguration promotion, string label, List<string> failures)
    {
        if (promotion.Tiers.Count == 0)
        {
            failures.Add($"{label}: at least one tier is required.");
            return;
        }

        if (promotion.Tiers.Select(tier => tier.MinimumOrderAmount).Distinct().Count() != promotion.Tiers.Count)
        {
            failures.Add($"{label}: tier MinimumOrderAmount values must be unique.");
        }

        foreach (var tier in promotion.Tiers)
        {
            if (tier.MinimumOrderAmount < 0m)
            {
                failures.Add($"{label}: tier MinimumOrderAmount cannot be negative.");
            }

            if (tier.DiscountAmount < 0m)
            {
                failures.Add($"{label}: tier DiscountAmount cannot be negative.");
            }
        }
    }
}
