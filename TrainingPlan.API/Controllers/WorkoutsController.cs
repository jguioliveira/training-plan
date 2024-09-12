using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TrainingPlan.API.Application.Features.WorkoutFeatures.CreateComment;
using TrainingPlan.API.Application.Features.WorkoutFeatures.CreateWorkout;
using TrainingPlan.API.Application.Features.WorkoutFeatures.GetWorkout;
using TrainingPlan.API.Application.Features.WorkoutFeatures.RemoveComment;
using TrainingPlan.API.Application.Features.WorkoutFeatures.UpdateComment;
using TrainingPlan.API.Application.Features.WorkoutFeatures.UpdateWorkout;
using TrainingPlan.Domain.DTO;

namespace TrainingPlan.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Creates a new workout.
        /// </summary>
        /// <param name="request">The request containing the workout details.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new workout.")]
        [SwaggerResponse(200, "Workout created successfully.")]
        [SwaggerResponse(400, "Invalid request.")]
        [HttpPost]
        public async Task<ActionResult<CreateWorkoutResponse>> CreateWorkoutAsync(CreateWorkoutRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Updates an existing workout.
        /// </summary>
        /// <param name="request">The request containing the updated workout details.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPut]
        public async Task<ActionResult<UpdateWorkoutResponse>> UpdateWorkoutAsync(UpdateWorkoutRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Gets a workout by ID.
        /// </summary>
        /// <param name="id">The ID of the workout.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The workout details.</returns>
        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Get workout by id")]
        [SwaggerResponse(404, "Workout was not found")]
        [SwaggerResponse(200, "Returns a specific workout", typeof(WorkoutDTO))]
        public async Task<ActionResult<WorkoutDTO>> GetWorkoutAsync(
            int id,
            CancellationToken cancellationToken)
        {
            GetWorkoutRequest request = new() { Id = id };

            var response = await _mediator.Send(request, cancellationToken);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        /// <summary>
        /// Creates a new comment for a specific workout.
        /// </summary>
        /// <param name="workoutId">The ID of the workout.</param>
        /// <param name="request">The request containing the comment details.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPost("{workoutId}/comments")]
        [SwaggerOperation(Summary = "Creates a new comment for a specific workout.")]
        [SwaggerResponse(200, "Comment created successfully.")]
        [SwaggerResponse(400, "Invalid request.")]
        public async Task<IActionResult> CreateComment([FromRoute] int workoutId, CreateWorkoutCommentRequest request)
        {
            request.WorkoutId = workoutId;

            var response = await _mediator.Send(request);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Removes a comment from a specific workout.
        /// </summary>
        /// <param name="workoutId">The ID of the workout.</param>
        /// <param name="commentId">The ID of the comment to be removed.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpDelete("{workoutId}/comments/{commentId}")]
        [SwaggerOperation(Summary = "Removes a comment from a specific workout.")]
        [SwaggerResponse(200, "Comment removed successfully.")]
        [SwaggerResponse(400, "Invalid request.")]
        [SwaggerResponse(404, "Comment or workout not found.")]
        public async Task<IActionResult> RemoveComment([FromRoute] int workoutId, [FromRoute] int commentId)
        {
            var request = new RemoveCommentRequest
            {
                WorkoutId = workoutId,
                CommentId = commentId
            };

            var response = await _mediator.Send(request);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Updates a comment for a specific workout.
        /// </summary>
        /// <param name="workoutId">The ID of the workout.</param>
        /// <param name="commentId">The ID of the comment to be updated.</param>
        /// <param name="request">The request containing the updated comment details.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPut("{workoutId}/comments/{commentId}")]
        [SwaggerOperation(Summary = "Updates a comment for a specific workout.")]
        [SwaggerResponse(200, "Comment updated successfully.")]
        [SwaggerResponse(400, "Invalid request.")]
        [SwaggerResponse(404, "Comment or workout not found.")]
        public async Task<IActionResult> UpdateComment([FromRoute] int workoutId, [FromRoute] int commentId, [FromBody] UpdateCommentRequest request)
        {
            request.WorkoutId = workoutId;
            request.CommentId = commentId;

            var response = await _mediator.Send(request);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
