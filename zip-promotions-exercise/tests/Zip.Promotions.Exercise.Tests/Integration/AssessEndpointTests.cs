using Zip.Promotions.Exercise.Testing;

namespace Zip.Promotions.Exercise.Tests.Integration;

/// <summary>
/// Drives the real HTTP surface. <see cref="PromotionApiFactory"/> boots the actual host against the real
/// <c>appsettings.json</c>, so these tests see the seeded promotions and customers described in the README —
/// <see cref="SeedIds"/> names them. Use <c>api.CreateClientFor(customerId)</c> for a client that carries the
/// customer header.
/// </summary>
[Trait("Category", "Integration")]
public class AssessEndpointTests : IClassFixture<PromotionApiFactory>
{
    private readonly PromotionApiFactory api;

    public AssessEndpointTests(PromotionApiFactory api) => this.api = api;
}
