using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TrainingPlan.API.Application.Features.AthleteFeatures.CreateAthlete;
using TrainingPlan.API.Application.Features.AthleteFeatures.GetAthlete;
using TrainingPlan.API.Application.Features.AthleteFeatures.GetAthletes;
using TrainingPlan.API.Application.Features.AthleteFeatures.UpdateAthlete;
using TrainingPlan.Domain.DTO;

namespace TrainingPlan.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AthletesController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        [SwaggerOperation(Summary = "Get paginated list of athletes")]
        [SwaggerResponse(200, "Returns the list of athletes", typeof(AthletesPagedListDTO))]
        public async Task<ActionResult<AthletesPagedListDTO>> GetAsync(
            CancellationToken cancellationToken,
            [FromQuery] string? name, 
            [FromHeader(Name = "pageSize")] int pageSize = 10,
            [FromHeader(Name = "lastId")] int lastRowId = 0,
            [FromHeader(Name = "direction")] string? direction = "ASC")
        {
            GetAthletesRequest request = new() { Direction = direction, Name = name, PageSize = pageSize, LastId = lastRowId };

            var response = await _mediator.Send(request, cancellationToken);

            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Get athlete by id")]
        [SwaggerResponse(200, "Returns a specific athlete", typeof(AthleteDTO))]
        [SwaggerResponse(404, "Athlete was not found")]
        public async Task<ActionResult<AthleteDTO>> GetByIdAsync(
            int id,
            CancellationToken cancellationToken)
        {
            GetAthleteRequest request = new() { Id = id };

            var response = await _mediator.Send(request, cancellationToken);

            if(response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<CreateAthleteResponse>> CreateAsync(CreateAthleteRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<UpdateAthleteResponse>> UpdateAsync(UpdateAthleteRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }
    }
}
