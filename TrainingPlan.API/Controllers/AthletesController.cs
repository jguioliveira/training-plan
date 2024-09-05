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

        /// <summary>
        /// Get paginated list of athletes.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="name">Filter by athlete name.</param>
        /// <param name="pageSize">Number of athletes per page.</param>
        /// <param name="lastRowId">ID of the last athlete in the previous page.</param>
        /// <param name="direction">Sort direction (ASC or DESC).</param>
        /// <returns>Paginated list of athletes.</returns>
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

        /// <summary>
        /// Get athlete by ID.
        /// </summary>
        /// <param name="id">The ID of the athlete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The athlete details.</returns>
        [HttpGet("{id}")]
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

        /// <summary>
        /// Creates a new athlete.
        /// </summary>
        /// <param name="request">The request containing the athlete details.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new athlete.")]
        [SwaggerResponse(200, "Athlete created successfully.")]
        [SwaggerResponse(400, "Invalid request.")]
        public async Task<ActionResult<CreateAthleteResponse>> CreateAsync(CreateAthleteRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Updates an existing athlete.
        /// </summary>
        /// <param name="request">The request containing the updated athlete details.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPut]
        [SwaggerOperation(Summary = "Updates an existing athlete.")]
        [SwaggerResponse(200, "Athlete updated successfully.")]
        [SwaggerResponse(400, "Invalid request.")]
        public async Task<ActionResult<UpdateAthleteResponse>> UpdateAsync(UpdateAthleteRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }
    }
}
