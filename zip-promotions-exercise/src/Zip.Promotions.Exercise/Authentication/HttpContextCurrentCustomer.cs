using System.Security.Claims;

namespace Zip.Promotions.Exercise.Authentication;

public sealed class HttpContextCurrentCustomer : ICurrentCustomer
{
    private readonly IHttpContextAccessor accessor;

    public HttpContextCurrentCustomer(IHttpContextAccessor accessor) => this.accessor = accessor;

    // The handler has already refused anything that is not a customer id, so a claim that fails to parse here is a bug.
    public Guid CustomerId =>
        Guid.TryParse(this.accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out var customerId)
            ? customerId
            : throw new InvalidOperationException("The current request has no authenticated customer.");
}
