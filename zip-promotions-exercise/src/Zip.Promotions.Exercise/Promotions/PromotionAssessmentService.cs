using Microsoft.Extensions.Logging;

using Zip.Promotions.Exercise.Authentication;

namespace Zip.Promotions.Exercise.Promotions;

public sealed class PromotionAssessmentService : IPromotionAssessmentService
{
    private readonly IPromotionEvaluator evaluator;
    private readonly ICurrentCustomer currentCustomer;
    private readonly ILogger<PromotionAssessmentService> logger;

    public PromotionAssessmentService(
        IPromotionEvaluator evaluator,
        ICurrentCustomer currentCustomer,
        ILogger<PromotionAssessmentService> logger)
    {
        this.evaluator = evaluator;
        this.currentCustomer = currentCustomer;
        this.logger = logger;
    }

    public async Task<PromotionAssessment> AssessAsync(
        string orderId,
        Guid merchantId,
        decimal orderAmount,
        CancellationToken cancellationToken = default)
    {
        var amount = Money.Round(orderAmount);

        var best = await this.evaluator
            .FindBestAsync(this.currentCustomer.CustomerId, merchantId, amount, cancellationToken)
            .ConfigureAwait(false);

        this.logger.LogInformation(
            "Assessment completed for order {OrderId} at merchant {MerchantId} on {OrderAmount}: selected {PromotionId}",
            orderId,
            merchantId,
            amount,
            best?.Promotion.Id.ToString() ?? "none");

        return best is null
            ? PromotionAssessment.None(amount)
            : new PromotionAssessment(true, best.Promotion.Id, best.Promotion.Name, best.DiscountAmount, amount);
    }
}
