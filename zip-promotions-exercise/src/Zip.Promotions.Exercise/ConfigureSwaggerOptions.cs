using Microsoft.Extensions.Options;
using Microsoft.OpenApi;

using Swashbuckle.AspNetCore.SwaggerGen;

using Zip.Promotions.Exercise.Authentication;
using Zip.Promotions.Exercise.Configuration;

namespace Zip.Promotions.Exercise;

public sealed class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IOptions<CustomerAuthenticationOptions> authenticationOptions;

    public ConfigureSwaggerOptions(IOptions<CustomerAuthenticationOptions> authenticationOptions) =>
        this.authenticationOptions = authenticationOptions;

    public void Configure(SwaggerGenOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Zip Promotions",
            Version = "v1",
            Description = "Assesses the best promotion for an order. Order lifecycle changes arrive on the message bus, not over HTTP.",
        });

        var documentation = Path.Combine(AppContext.BaseDirectory, "Zip.Promotions.Exercise.xml");
        if (File.Exists(documentation))
        {
            options.IncludeXmlComments(documentation);
        }

        // Nested types would otherwise produce '+' in their schema identifiers.
        options.CustomSchemaIds(type => type.FullName?.Replace('+', '.'));

        options.AddSecurityDefinition(
            CustomerHeaderAuthenticationHandler.SchemeName,
            new OpenApiSecurityScheme
            {
                Name = this.authenticationOptions.Value.CustomerHeaderName,
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Description = "Stand-in for customer authentication. Any non-empty GUID works.",
            });

        options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference(CustomerHeaderAuthenticationHandler.SchemeName, document)] = [],
        });
    }
}
