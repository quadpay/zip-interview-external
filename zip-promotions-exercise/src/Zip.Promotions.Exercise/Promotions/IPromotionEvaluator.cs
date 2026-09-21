using Zip.Promotions.Exercise.Eligibility;

namespace Zip.Promotions.Exercise.Promotions;

public interface IPromotionEvaluator
{
    Task<PromotionEvaluation?> FindBestAsync(
        Guid customerId,
        Guid merchantId,
        decimal orderAmount,
        CancellationToken cancellationToken = default);
}
