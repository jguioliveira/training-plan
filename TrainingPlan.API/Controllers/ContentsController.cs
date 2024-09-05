using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TrainingPlan.API.Application.Features.ContentFeatures.CreateContent;
using TrainingPlan.API.Application.Features.ContentFeatures.GetAllContents;
using TrainingPlan.API.Application.Features.ContentFeatures.GetContent;
using TrainingPlan.API.Application.Features.ContentFeatures.UpdateContent;
using TrainingPlan.Domain.DTO;

namespace TrainingPlan.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Get paginated list of contents.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="title">Filter by content title.</param>
        /// <param name="pageSize">Number of contents per page.</param>
        /// <param name="lastRowId">ID of the last content in the previous page.</param>
        /// <param name="direction">Sort direction (ASC or DESC).</param>
        /// <returns>Paginated list of contents.</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Get paginated list of contents")]
        [SwaggerResponse(200, "Returns the list of contents", typeof(ContentsPagedListDTO))]
        public async Task<ActionResult<ContentsPagedListDTO>> GetAsync(
            CancellationToken cancellationToken,
            [FromQuery] string? title,
            [FromHeader(Name = "pageSize")] int pageSize = 10,
            [FromHeader(Name = "lastId")] int lastRowId = 0,
            [FromHeader(Name = "direction")] string? direction = "ASC")
        {
            GetAllContentsRequest request = new() { Direction = direction, Title = title, PageSize = pageSize, LastId = lastRowId };

            var response = await _mediator.Send(request, cancellationToken);

            return Ok(response);
        }

        /// <summary>
        /// Get content by ID.
        /// </summary>
        /// <param name="id">The ID of the content.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The content details.</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get content by id")]
        [SwaggerResponse(200, "Returns a specific content", typeof(ContentDTO))]
        [SwaggerResponse(404, "Content was not found")]
        public async Task<ActionResult<ContentDTO>> GetByIdAsync(
            int id,
            CancellationToken cancellationToken)
        {
            GetContentRequest request = new() { Id = id };

            var response = await _mediator.Send(request, cancellationToken);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        /// <summary>
        /// Creates a new content.
        /// </summary>
        /// <param name="request">The request containing the content details.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new content.")]
        [SwaggerResponse(200, "Content created successfully.")]
        [SwaggerResponse(400, "Invalid request.")]
        public async Task<ActionResult<CreateContentResponse>> CreateAsync(CreateContentRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Updates an existing content.
        /// </summary>
        /// <param name="request">The request containing the updated content details.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPut]
        [SwaggerOperation(Summary = "Updates an existing content.")]
        [SwaggerResponse(200, "Content updated successfully.")]
        [SwaggerResponse(400, "Invalid request.")]
        public async Task<ActionResult<UpdateContentResponse>> UpdateAsync(UpdateContentRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }
    }
}
