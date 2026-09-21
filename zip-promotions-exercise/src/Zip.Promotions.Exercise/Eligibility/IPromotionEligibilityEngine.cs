using Zip.Promotions.Exercise.Promotions;

namespace Zip.Promotions.Exercise.Eligibility;

public interface IPromotionEligibilityEngine
{
    ValueTask<IReadOnlyList<PromotionEvaluation>> EvaluateAllAsync(
        IEnumerable<Promotion> promotions,
        PromotionEligibilityContext context,
        CancellationToken cancellationToken);
}
