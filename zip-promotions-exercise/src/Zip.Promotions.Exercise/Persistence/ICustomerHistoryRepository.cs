using Zip.Promotions.Exercise.Customers;

namespace Zip.Promotions.Exercise.Persistence;

public interface ICustomerHistoryRepository
{
    Task<CustomerOrderHistorySummary> GetSummaryAsync(
        Guid customerId,
        Guid merchantId,
        CancellationToken cancellationToken = default);
}
