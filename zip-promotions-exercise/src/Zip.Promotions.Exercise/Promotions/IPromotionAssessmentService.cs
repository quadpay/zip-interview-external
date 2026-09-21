namespace Zip.Promotions.Exercise.Promotions;

public interface IPromotionAssessmentService
{
    Task<PromotionAssessment> AssessAsync(
        string orderId,
        Guid merchantId,
        decimal orderAmount,
        CancellationToken cancellationToken = default);
}
