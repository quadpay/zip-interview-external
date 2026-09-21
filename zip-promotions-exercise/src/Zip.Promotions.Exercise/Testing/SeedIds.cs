namespace Zip.Promotions.Exercise.Testing;

/// <summary>The ids seeded in <c>appsettings.json</c>, so tests can name them instead of pasting GUIDs.</summary>
public static class SeedIds
{
    public static readonly Guid GlobalTenOff = new("a1000000-0000-0000-0000-000000000001");
    public static readonly Guid WelcomeTwentyFive = new("a1000000-0000-0000-0000-000000000002");
    public static readonly Guid NikeTiered = new("a1000000-0000-0000-0000-000000000003");
    public static readonly Guid NikeNewcomer = new("a1000000-0000-0000-0000-000000000004");
    public static readonly Guid LoyaltyFifteen = new("a1000000-0000-0000-0000-000000000005");
    public static readonly Guid RetiredFiftyOff = new("a1000000-0000-0000-0000-000000000006");
    public static readonly Guid WinBackTwenty = new("a1000000-0000-0000-0000-000000000007");

    public static readonly Guid Nike = new("b2000000-0000-0000-0000-000000000001");
    public static readonly Guid Adidas = new("b2000000-0000-0000-0000-000000000002");

    /// <summary>A merchant with no promotions of its own, for exercising the merchant-agnostic ones.</summary>
    public static readonly Guid NewBalance = new("b2000000-0000-0000-0000-000000000003");

    /// <summary>No order history at all, so new to Zip and new to every merchant.</summary>
    public static readonly Guid NewCustomer = new("c3000000-0000-0000-0000-000000000001");

    /// <summary>Three confirmed orders at Nike and two at Adidas, the most recent of them recent.</summary>
    public static readonly Guid ExistingCustomer = new("c3000000-0000-0000-0000-000000000002");

    /// <summary>Two confirmed orders at Nike, but none since January 2024.</summary>
    public static readonly Guid LapsedCustomer = new("c3000000-0000-0000-0000-000000000003");
}
