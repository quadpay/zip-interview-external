using System.Security.Claims;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

using Zip.Promotions.Exercise.Configuration;

namespace Zip.Promotions.Exercise.Authentication;

// Stands in for the real token pipeline: the caller asserts its identity with a configured header.
public sealed class CustomerHeaderAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "CustomerHeader";

    private readonly IOptionsMonitor<CustomerAuthenticationOptions> customerOptions;

    public CustomerHeaderAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory loggerFactory,
        UrlEncoder encoder,
        IOptionsMonitor<CustomerAuthenticationOptions> customerOptions)
        : base(options, loggerFactory, encoder) =>
        this.customerOptions = customerOptions;

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var headerName = this.customerOptions.CurrentValue.CustomerHeaderName;

        if (!this.Request.Headers.TryGetValue(headerName, out var header))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        if (!Guid.TryParse(header.ToString(), out var customerId) || customerId == Guid.Empty)
        {
            return Task.FromResult(AuthenticateResult.Fail($"The '{headerName}' header was not a customer id."));
        }

        var identity = new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, customerId.ToString())], SchemeName);
        var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
