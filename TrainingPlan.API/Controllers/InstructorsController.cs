using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TrainingPlan.API.Application.Features.InstructorFeatures.CreateInstructor;
using TrainingPlan.API.Application.Features.InstructorFeatures.GetInstructor;
using TrainingPlan.API.Application.Features.InstructorFeatures.GetAllInstructors;
using TrainingPlan.API.Application.Features.InstructorFeatures.UpdateInstructor;
using TrainingPlan.Domain.DTO;

namespace TrainingPlan.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Get paginated list of instructors.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="name">Filter by instructor name.</param>
        /// <param name="pageSize">Number of instructors per page.</param>
        /// <param name="lastRowId">ID of the last instructor in the previous page.</param>
        /// <param name="direction">Sort direction (ASC or DESC).</param>
        /// <returns>Paginated list of instructors.</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Get paginated list of instructors")]
        [SwaggerResponse(200, "Returns the list of instructors", typeof(InstructorsPagedListDTO))]
        public async Task<ActionResult<InstructorsPagedListDTO>> GetAsync(
            CancellationToken cancellationToken,
            [FromQuery] string? name,
            [FromHeader(Name = "pageSize")] int pageSize = 10,
            [FromHeader(Name = "lastId")] int lastRowId = 0,
            [FromHeader(Name = "direction")] string? direction = "ASC")
        {
            GetAllInstructorsRequest request = new() { Direction = direction, Name = name, PageSize = pageSize, LastId = lastRowId };

            var response = await _mediator.Send(request, cancellationToken);

            return Ok(response);
        }

        /// <summary>
        /// Get instructor by ID.
        /// </summary>
        /// <param name="id">The ID of the instructor.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The instructor details.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get instructor by id")]
        [SwaggerResponse(200, "Returns a specific instructor", typeof(InstructorDTO))]
        [SwaggerResponse(404, "Instructor was not found")]
        public async Task<ActionResult<InstructorDTO>> GetByIdAsync(
            int id,
            CancellationToken cancellationToken)
        {
            GetInstructorRequest request = new() { Id = id };

            var response = await _mediator.Send(request, cancellationToken);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        /// <summary>
        /// Creates a new instructor.
        /// </summary>
        /// <param name="request">The request containing the instructor details.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new instructor.")]
        [SwaggerResponse(200, "Instructor created successfully.")]
        [SwaggerResponse(400, "Invalid request.")]
        public async Task<ActionResult<CreateInstructorResponse>> CreateAsync(CreateInstructorRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Updates an existing instructor.
        /// </summary>
        /// <param name="request">The request containing the updated instructor details.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPut]
        [SwaggerOperation(Summary = "Updates an existing instructor.")]
        [SwaggerResponse(200, "Instructor updated successfully.")]
        [SwaggerResponse(400, "Invalid request.")]
        public async Task<ActionResult<UpdateInstructorResponse>> UpdateAsync(UpdateInstructorRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }
    }
}
