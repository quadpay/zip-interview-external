namespace Zip.Promotions.Exercise.Configuration;

public sealed class CustomerOrderHistorySeed
{
    public Guid CustomerId { get; init; }

    public Guid MerchantId { get; init; }

    public int ConfirmedOrderCount { get; init; }

    public DateTimeOffset LastConfirmedAt { get; init; }
}
