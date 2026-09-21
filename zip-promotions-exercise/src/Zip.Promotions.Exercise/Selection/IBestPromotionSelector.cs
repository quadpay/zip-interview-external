using Zip.Promotions.Exercise.Eligibility;

namespace Zip.Promotions.Exercise.Selection;

public interface IBestPromotionSelector
{
    PromotionEvaluation? SelectBest(IEnumerable<PromotionEvaluation> evaluations);
}
