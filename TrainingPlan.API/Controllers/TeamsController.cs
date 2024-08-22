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
    public class TeamsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Get team by id")]
        [SwaggerResponse(404, "Team was not found")]
        [SwaggerResponse(200, "Returns a specific team", typeof(TeamDTO))]
        public async Task<ActionResult<TeamDTO>> GetByIdAsync(
            int id,
            CancellationToken cancellationToken)
        {
            GetTeamRequest request = new() { Id = id };

            var response = await _mediator.Send(request, cancellationToken);

            if (response == null) 
            { 
                return NotFound();
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<CreateTeamResponse>> CreateAsync(CreateTeamRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<UpdateTeamResponse>> UpdateAsync(UpdateTeamRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpPost]
        [Route("{Id}/settings")]
        public async Task<ActionResult<SaveTeamSettingsResponse>> SaveSettingsAsync(SaveTeamSettingsRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpPost]
        [Route("{Id}/social-media")]
        public async Task<ActionResult<SaveTeamSocialMediaResponse>> SaveSocialMediaAsync(SaveTeamSocialMediaRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpDelete]
        [Route("{Id}/social-media")]
        public async Task<ActionResult<DeleteTeamSocialMediaResponse>> DeleteSocialMediaAsync(DeleteTeamSocialMediaRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }
    }
}
