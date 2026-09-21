using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Zip.Promotions.Exercise.Contracts;
using Zip.Promotions.Exercise.Promotions;

namespace Zip.Promotions.Exercise.Controllers;

[ApiController]
[Authorize]
[Route("promotions")]
public sealed class PromotionsController : ControllerBase
{
    private readonly IPromotionAssessmentService assessments;

    public PromotionsController(IPromotionAssessmentService assessments) => this.assessments = assessments;

    /// <summary>
    /// Returns the best promotion currently available for an order.
    /// </summary>
    [HttpPost("assess")]
    [ProducesResponseType<PromotionAssessment>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PromotionAssessment>> Assess(
        AssessPromotionRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return this.Ok(await this.assessments.AssessAsync(
            request.OrderId,
            request.MerchantId,
            request.OrderAmount,
            cancellationToken));
    }
}
