using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TrainingPlan.API.Application.Features.TeamFeatures.CreateTeam;
using TrainingPlan.API.Application.Features.TeamFeatures.DeleteTeamSocialMedia;
using TrainingPlan.API.Application.Features.TeamFeatures.GetTeam;
using TrainingPlan.API.Application.Features.TeamFeatures.SaveTeamSettings;
using TrainingPlan.API.Application.Features.TeamFeatures.SaveTeamSocialMedia;
using TrainingPlan.API.Application.Features.TeamFeatures.UpdateTeam;
using TrainingPlan.Domain.DTO;

namespace TrainingPlan.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TeamsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get team by ID.
        /// </summary>
        /// <param name="teamId">The ID of the team.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The team details.</returns>
        [HttpGet]
        [Route("{teamId}")]
        [SwaggerOperation(Summary = "Get team by id")]
        [SwaggerResponse(404, "Team was not found")]
        [SwaggerResponse(200, "Returns a specific team", typeof(TeamDTO))]
        public async Task<ActionResult<TeamDTO>> GetByIdAsync(
            int teamId,
            CancellationToken cancellationToken)
        {
            GetTeamRequest request = new() { Id = teamId };

            var response = await _mediator.Send(request, cancellationToken);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        /// <summary>
        /// Creates a new team.
        /// </summary>
        /// <param name="request">The request containing the team details.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new team.")]
        [SwaggerResponse(200, "Team created successfully.")]
        [SwaggerResponse(400, "Invalid request.")]
        public async Task<ActionResult<CreateTeamResponse>> CreateAsync(CreateTeamRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Updates an existing team.
        /// </summary>
        /// <param name="request">The request containing the updated team details.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPut]
        [SwaggerOperation(Summary = "Updates an existing team.")]
        [SwaggerResponse(200, "Team updated successfully.")]
        [SwaggerResponse(400, "Invalid request.")]
        public async Task<ActionResult<UpdateTeamResponse>> UpdateAsync(UpdateTeamRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Saves team settings.
        /// </summary>
        /// <param name="teamId">The ID of the team.</param>
        /// <param name="request">The request containing the team settings details.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPost]
        [Route("{teamId}/settings")]
        [SwaggerOperation(Summary = "Saves team settings.")]
        [SwaggerResponse(200, "Team settings saved successfully.")]
        [SwaggerResponse(400, "Invalid request.")]
        public async Task<ActionResult<SaveTeamSettingsResponse>> SaveSettingsAsync(SaveTeamSettingsRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Saves team social media details.
        /// </summary>
        /// <param name="request">The request containing the team social media details.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPost]
        [Route("{Id}/social-media")]
        [SwaggerOperation(Summary = "Saves team social media details.")]
        [SwaggerResponse(200, "Team social media details saved successfully.")]
        [SwaggerResponse(400, "Invalid request.")]
        public async Task<ActionResult<SaveTeamSocialMediaResponse>> SaveSocialMediaAsync(SaveTeamSocialMediaRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Deletes team social media details.
        /// </summary>
        /// <param name="request">The request containing the team social media details to be deleted.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpDelete]
        [Route("{Id}/social-media")]
        [SwaggerOperation(Summary = "Deletes team social media details.")]
        [SwaggerResponse(200, "Team social media details deleted successfully.")]
        [SwaggerResponse(400, "Invalid request.")]
        public async Task<ActionResult<DeleteTeamSocialMediaResponse>> DeleteSocialMediaAsync(DeleteTeamSocialMediaRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }
    }
}