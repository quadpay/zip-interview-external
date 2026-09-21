using Zip.Promotions.Exercise.Eligibility;
using Zip.Promotions.Exercise.Persistence;
using Zip.Promotions.Exercise.Selection;

namespace Zip.Promotions.Exercise.Promotions;

public sealed class PromotionEvaluator : IPromotionEvaluator
{
    private readonly IPromotionRepository promotions;
    private readonly ICustomerHistoryRepository customerHistory;
    private readonly IPromotionEligibilityEngine engine;
    private readonly IBestPromotionSelector selector;
    private readonly TimeProvider timeProvider;

    public PromotionEvaluator(
        IPromotionRepository promotions,
        ICustomerHistoryRepository customerHistory,
        IPromotionEligibilityEngine engine,
        IBestPromotionSelector selector,
        TimeProvider timeProvider)
    {
        this.promotions = promotions;
        this.customerHistory = customerHistory;
        this.engine = engine;
        this.selector = selector;
        this.timeProvider = timeProvider;
    }

    public async Task<PromotionEvaluation?> FindBestAsync(
        Guid customerId,
        Guid merchantId,
        decimal orderAmount,
        CancellationToken cancellationToken = default)
    {
        var candidates = await this.promotions.GetActiveAsync(cancellationToken).ConfigureAwait(false);

        var history = await this.customerHistory
            .GetSummaryAsync(customerId, merchantId, cancellationToken)
            .ConfigureAwait(false);

        var context = new PromotionEligibilityContext(
            customerId,
            merchantId,
            orderAmount,
            this.timeProvider.GetUtcNow(),
            history);

        var evaluations = await this.engine
            .EvaluateAllAsync(candidates, context, cancellationToken)
            .ConfigureAwait(false);

        return this.selector.SelectBest(evaluations);
    }
}
