namespace Zip.Promotions.Exercise;

public static class Money
{
    public const int Decimals = 2;

    public static decimal Round(decimal amount) => Math.Round(amount, Decimals, MidpointRounding.AwayFromZero);
}
