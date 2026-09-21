using System.Collections.Concurrent;

using Microsoft.Extensions.Options;

using Zip.Promotions.Exercise.Configuration;
using Zip.Promotions.Exercise.Customers;

namespace Zip.Promotions.Exercise.Persistence;

/// <summary>
/// Stands in for the customer order history table, seeded from the <c>Seed</c> configuration section.
/// One row per customer and merchant, holding how many orders that customer has confirmed with that merchant
/// and when the most recent of them was.
/// </summary>
public sealed class FakeCustomerHistoryRepository : ICustomerHistoryRepository
{
    private readonly ConcurrentDictionary<(Guid CustomerId, Guid MerchantId), OrderHistoryRow> rows;

    public FakeCustomerHistoryRepository(IOptions<SeedOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options);

        this.rows = new ConcurrentDictionary<(Guid, Guid), OrderHistoryRow>(
            options.Value.CustomerOrderHistory.ToDictionary(
                seed => (seed.CustomerId, seed.MerchantId),
                seed => new OrderHistoryRow(seed.ConfirmedOrderCount, seed.LastConfirmedAt)));
    }

    public Task<CustomerOrderHistorySummary> GetSummaryAsync(
        Guid customerId,
        Guid merchantId,
        CancellationToken cancellationToken = default)
    {
        var withZip = 0;
        DateTimeOffset? lastWithZip = null;

        foreach (var (key, row) in this.rows)
        {
            if (key.CustomerId != customerId)
            {
                continue;
            }

            withZip += row.ConfirmedOrderCount;

            if (lastWithZip is null || row.LastConfirmedAt > lastWithZip)
            {
                lastWithZip = row.LastConfirmedAt;
            }
        }

        var withMerchant = this.rows.TryGetValue((customerId, merchantId), out var merchantRow)
            ? merchantRow.ConfirmedOrderCount
            : 0;

        return Task.FromResult(new CustomerOrderHistorySummary(withZip, withMerchant, lastWithZip));
    }

    private sealed record OrderHistoryRow(int ConfirmedOrderCount, DateTimeOffset LastConfirmedAt);
}
