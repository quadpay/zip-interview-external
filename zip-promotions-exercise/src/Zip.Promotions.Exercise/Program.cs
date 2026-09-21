using System.Diagnostics.CodeAnalysis;

using Zip.Promotions.Exercise;

[ExcludeFromCodeCoverage]
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddPromotions(builder.Configuration)
            .AddPromotionApi();

        var app = builder.Build();

        // The exercise is local-only, so the explorer is always on rather than gated behind an environment check.
        app.UseSwagger();
        app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Zip Promotions v1"));

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.MapHealthChecks("/health");

        app.Run();
    }
}
