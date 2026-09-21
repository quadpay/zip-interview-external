using FluentValidation;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Zip.Promotions.Exercise.Validation;

/// <summary>
/// Runs the validator registered for each action argument before the action does, so a controller only ever sees a
/// request that has already been checked. Arguments with no registered validator pass straight through.
/// </summary>
public sealed class ValidationFilter : IAsyncActionFilter
{
    private readonly IServiceProvider services;

    public ValidationFilter(IServiceProvider services) => this.services = services;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(next);

        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null || this.ValidatorFor(argument) is not { } validator)
            {
                continue;
            }

            var result = await validator
                .ValidateAsync(new ValidationContext<object>(argument), context.HttpContext.RequestAborted)
                .ConfigureAwait(false);

            foreach (var failure in result.Errors)
            {
                context.ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
            }
        }

        if (!context.ModelState.IsValid)
        {
            context.Result = new BadRequestObjectResult(new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest,
            });

            return;
        }

        await next().ConfigureAwait(false);
    }

    private IValidator? ValidatorFor(object argument) =>
        this.services.GetService(typeof(IValidator<>).MakeGenericType(argument.GetType())) as IValidator;
}
