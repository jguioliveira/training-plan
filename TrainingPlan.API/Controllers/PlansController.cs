using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TrainingPlan.API.Application.Features.PlanFeatures.CreatePlan;
using TrainingPlan.API.Application.Features.PlanFeatures.CreateWorkout;
using TrainingPlan.API.Application.Features.PlanFeatures.GetPlan;
using TrainingPlan.API.Application.Features.PlanFeatures.GetWorkout;
using TrainingPlan.API.Application.Features.PlanFeatures.UpdatePlan;
using TrainingPlan.API.Application.Features.PlanFeatures.UpdateWorkout;
using TrainingPlan.Domain.DTO;

namespace TrainingPlan.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlansController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

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

        [HttpPost]
        public async Task<ActionResult<CreatePlanResponse>> CreateAsync(CreatePlanRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<UpdatePlanResponse>> UpdateAsync(UpdatePlanRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpPost]
        [Route("{Id}/workout")]
        public async Task<ActionResult<CreateWorkoutResponse>> CreateWorkoutAsync(CreatePlanWorkoutRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpPut]
        [Route("{Id}/workout")]
        public async Task<ActionResult<UpdateWorkoutResponse>> UpdateWorkoutAsync(UpdatePlanWorkoutRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}/workout/{idWorkout}")]
        [SwaggerOperation(Summary = "Get workout by id")]
        [SwaggerResponse(404, "Workout was not found")]
        [SwaggerResponse(200, "Returns a specific workout", typeof(WorkoutDTO))]
        public async Task<ActionResult<WorkoutDTO>> GetWorkoutAsync(
            int id, 
            int idWorkout,
            CancellationToken cancellationToken)
        {
            GetWorkoutRequest request = new() { IdWorkout = idWorkout };

            var response = await _mediator.Send(request, cancellationToken);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }
    }
}
