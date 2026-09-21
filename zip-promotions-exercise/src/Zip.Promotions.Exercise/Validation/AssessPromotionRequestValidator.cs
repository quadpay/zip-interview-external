using FluentValidation;

using Zip.Promotions.Exercise.Contracts;

namespace Zip.Promotions.Exercise.Validation;

public sealed class AssessPromotionRequestValidator : AbstractValidator<AssessPromotionRequest>
{
    public AssessPromotionRequestValidator()
    {
        this.RuleFor(request => request.OrderId).NotEmpty();

        // NotEmpty on a Guid refuses the all-zeroes value, which is what an omitted merchant binds to.
        this.RuleFor(request => request.MerchantId).NotEmpty();

        this.RuleFor(request => request.OrderAmount)
            .InclusiveBetween(MoneyRange.SmallestOrderAmount, MoneyRange.LargestAmount);
    }
}
