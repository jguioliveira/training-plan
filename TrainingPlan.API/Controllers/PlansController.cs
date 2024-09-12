using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TrainingPlan.API.Application.Features.PlanFeatures.CreatePlan;
using TrainingPlan.API.Application.Features.PlanFeatures.GetPlan;
using TrainingPlan.API.Application.Features.PlanFeatures.UpdatePlan;
using TrainingPlan.Domain.DTO;

namespace TrainingPlan.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlansController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Get paginated list of plans
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="pageSize">Number of athletes per page.</param>
        /// <param name="lastRowId">ID of the last athlete in the previous page.</param>
        /// <param name="direction">Sort direction (ASC or DESC).</param>
        /// <returns>Paginated list of plans.</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Get plans")]
        [SwaggerResponse(404, "Plan was not found")]
        [SwaggerResponse(200, "Returns all plans", typeof(PlanDTO))]
        public async Task<ActionResult<PlanDTO>> GetAllAsync(
            CancellationToken cancellationToken,
            [FromHeader(Name = "pageSize")] int pageSize = 10,
            [FromHeader(Name = "lastId")] int lastRowId = 0,
            [FromHeader(Name = "direction")] string? direction = "ASC")
        {
            GetAllPlansRequest request = new() { Direction = direction, PageSize = pageSize, LastId = lastRowId };

            var response = await _mediator.Send(request, cancellationToken);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        /// <summary>
        /// Get plan by athlete ID.
        /// </summary>
        /// <param name="athleteId">The ID of the athlete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The plan details.</returns>
        [HttpGet]
        [Route("athlete/{athleteId}")]
        [SwaggerOperation(Summary = "Get plan by athlete id")]
        [SwaggerResponse(404, "Plan was not found")]
        [SwaggerResponse(200, "Returns athlete's plan", typeof(PlanDTO))]
        public async Task<ActionResult<PlanDTO>> GetByAthleteIdAsync(
            int athleteId,
            CancellationToken cancellationToken)
        {
            GetPlanRequest request = new() { AthleteId = athleteId };

            var response = await _mediator.Send(request, cancellationToken);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        /// <summary>
        /// Creates a new plan.
        /// </summary>
        /// <param name="request">The request containing the plan details.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new plan.")]
        [SwaggerResponse(200, "Plan created successfully.")]
        [SwaggerResponse(400, "Invalid request.")]
        public async Task<ActionResult<CreatePlanResponse>> CreateAsync(CreatePlanRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Updates an existing plan.
        /// </summary>
        /// <param name="request">The request containing the updated plan details.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPut]
        [SwaggerOperation(Summary = "Updates an existing plan.")]
        [SwaggerResponse(200, "Plan updated successfully.")]
        [SwaggerResponse(400, "Invalid request.")]
        public async Task<ActionResult<UpdatePlanResponse>> UpdateAsync(UpdatePlanRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }
    }
}
