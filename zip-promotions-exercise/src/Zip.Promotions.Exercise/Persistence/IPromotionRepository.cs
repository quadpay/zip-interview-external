using Zip.Promotions.Exercise.Promotions;

namespace Zip.Promotions.Exercise.Persistence;

public interface IPromotionRepository
{
    /// <summary>Every promotion that is currently active.</summary>
    Task<IReadOnlyList<Promotion>> GetActiveAsync(CancellationToken cancellationToken = default);
}
