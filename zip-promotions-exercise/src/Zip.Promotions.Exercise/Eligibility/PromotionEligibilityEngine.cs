using Zip.Promotions.Exercise.Promotions;

namespace Zip.Promotions.Exercise.Eligibility;

public sealed class PromotionEligibilityEngine : IPromotionEligibilityEngine
{
    public ValueTask<PromotionEvaluation> EvaluateAsync(
        Promotion promotion,
        PromotionEligibilityContext context,
        CancellationToken cancellationToken)
    {
        // TODO(candidate): Task 2 in EXERCISE.md.
        // Run each rule over this promotion and stop at the first one that says no, returning
        // PromotionEvaluation.Ineligible with the reason it gave. Nothing runs here today, which is why
        // nothing is ever ruled out.

        if (promotion.ResolveTier(context.OrderAmount) is null)
        {
            return ValueTask.FromResult(
                PromotionEvaluation.Ineligible(promotion, IneligibilityReason.BelowMinimumOrderAmount));
        }

        return ValueTask.FromResult(
            PromotionEvaluation.Eligible(promotion, promotion.CalculateDiscount(context.OrderAmount)));
    }

    public async ValueTask<IReadOnlyList<PromotionEvaluation>> EvaluateAllAsync(
        IEnumerable<Promotion> promotions,
        PromotionEligibilityContext context,
        CancellationToken cancellationToken)
    {
        var evaluations = new List<PromotionEvaluation>();

        foreach (var promotion in promotions)
        {
            evaluations.Add(await this.EvaluateAsync(promotion, context, cancellationToken).ConfigureAwait(false));
        }

        return evaluations;
    }
}
