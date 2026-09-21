namespace Zip.Promotions.Exercise.Customers;

public sealed record CustomerOrderHistorySummary(
    int ConfirmedOrdersWithZip,
    int ConfirmedOrdersWithMerchant,
    DateTimeOffset? LastConfirmedWithZip = null)
{
    public static CustomerOrderHistorySummary None { get; } = new(0, 0);

    public bool IsNewToZip => this.ConfirmedOrdersWithZip == 0;

    public bool IsNewToMerchant => this.ConfirmedOrdersWithMerchant == 0;
}
