using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Time.Testing;

namespace Zip.Promotions.Exercise.Testing;

/// <summary>
/// Boots the real host against the real <c>appsettings.json</c>, so anything written against this sees the seeded
/// promotions and customers. Construct one per test class, or one per test if you need a clean slate.
/// </summary>
/// <remarks>
/// The clock is frozen at <see cref="FrozenNow"/> so the dates in configuration stay fixed relative to "now",
/// whatever today happens to be. Advance <see cref="Time"/> to move it.
/// </remarks>
public sealed class PromotionApiFactory : WebApplicationFactory<Program>
{
    public const string CustomerHeaderName = "X-Customer-Id";

    public static readonly DateTimeOffset FrozenNow = new(2026, 6, 1, 12, 0, 0, TimeSpan.Zero);

    public FakeTimeProvider Time { get; } = new(FrozenNow);

    /// <summary>A client carrying the customer header, which is all the authentication this service has.</summary>
    public HttpClient CreateClientFor(Guid customerId)
    {
        var client = this.CreateClient();
        client.DefaultRequestHeaders.Add(CustomerHeaderName, customerId.ToString());

        return client;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.UseEnvironment(Environments.Development);
        builder.UseSetting("Logging:LogLevel:Default", "Warning");
        builder.UseSetting("Logging:LogLevel:Zip.Promotions.Exercise", "Warning");
        builder.ConfigureTestServices(services => services.AddSingleton<TimeProvider>(this.Time));
    }
}
