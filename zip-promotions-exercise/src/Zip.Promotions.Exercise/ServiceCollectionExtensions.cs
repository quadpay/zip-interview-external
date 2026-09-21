using System.Text.Json.Serialization;

using FluentValidation;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

using Swashbuckle.AspNetCore.SwaggerGen;

using Zip.Promotions.Exercise.Authentication;
using Zip.Promotions.Exercise.Caching;
using Zip.Promotions.Exercise.Configuration;
using Zip.Promotions.Exercise.Eligibility;
using Zip.Promotions.Exercise.Persistence;
using Zip.Promotions.Exercise.Promotions;
using Zip.Promotions.Exercise.Selection;
using Zip.Promotions.Exercise.Validation;

namespace Zip.Promotions.Exercise;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// The promotion engine: rules, services, storage and options. Nothing here needs a web host, so tests can
    /// register this on its own and drive the real graph in process.
    /// </summary>
    public static IServiceCollection AddPromotions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(TimeProvider.System);

        services.AddPromotionOptions(configuration);

        services.AddScoped<IPromotionEvaluator, PromotionEvaluator>();
        services.AddScoped<IPromotionAssessmentService, PromotionAssessmentService>();

        services.AddSingleton<IPromotionEligibilityEngine, PromotionEligibilityEngine>();
        services.AddSingleton<IBestPromotionSelector, BestPromotionSelector>();
        services.AddSingleton<FakeRedisCache>();
        services.AddSingleton<ICache>(provider => provider.GetRequiredService<FakeRedisCache>());

        services.AddSingleton<FakePromotionRepository>();
        services.AddSingleton<IPromotionRepository>(provider => new CachedPromotionRepository(
            provider.GetRequiredService<FakePromotionRepository>(),
            provider.GetRequiredService<ICache>(),
            provider.GetRequiredService<IOptionsMonitor<CacheOptions>>()));

        services.AddSingleton<ICustomerHistoryRepository, FakeCustomerHistoryRepository>();

        return services;
    }

    /// <summary>The HTTP surface: controllers, request validation, stand-in authentication and Swagger.</summary>
    public static IServiceCollection AddPromotionApi(this IServiceCollection services)
    {
        // Enums cross the wire as names, so Status and Audience read as "Active" rather than an opaque ordinal.
        services
            .AddControllers(options => options.Filters.Add<ValidationFilter>())
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        // Request validation lives in validator classes rather than attributes on the contracts, so the rules can be
        // composed, injected and unit tested on their own.
        services.AddValidatorsFromAssemblyContaining<AssessPromotionRequestValidator>(ServiceLifetime.Singleton);

        services.AddHealthChecks();

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentCustomer, HttpContextCurrentCustomer>();

        services
            .AddAuthentication(CustomerHeaderAuthenticationHandler.SchemeName)
            .AddScheme<AuthenticationSchemeOptions, CustomerHeaderAuthenticationHandler>(
                CustomerHeaderAuthenticationHandler.SchemeName,
                configureOptions: null);

        services.AddAuthorization();

        services.AddEndpointsApiExplorer();
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen();

        return services;
    }

    private static void AddPromotionOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<CacheOptions>()
            .Bind(configuration.GetSection(CacheOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<CustomerAuthenticationOptions>()
            .Bind(configuration.GetSection(CustomerAuthenticationOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<SeedOptions>()
            .Bind(configuration.GetSection(SeedOptions.SectionName));

        // Promotion rules are richer than data annotations express, so validation lives in a dedicated validator.
        services.AddOptions<PromotionOptions>()
            .Bind(configuration.GetSection(PromotionOptions.SectionName))
            .ValidateOnStart();

        services.AddSingleton<IValidateOptions<PromotionOptions>, PromotionOptionsValidator>();
    }
}
