using MediatR;
using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Repositories;

namespace TrainingPlan.API.Application.Features.ContentFeatures.GetAllContents
{
    public class GetAllContentsHandler(IContentRepository contentRepository) : IRequestHandler<GetAllContentsRequest, ContentsPagedListDTO>
    {
        private readonly IContentRepository _contentRepository = contentRepository;

        public Task<ContentsPagedListDTO> Handle(GetAllContentsRequest request, CancellationToken cancellationToken)
        {
            return _contentRepository.GetContentAsync(request.LastId, request.PageSize, request.Direction, request.Title);
        }
    }

    public record GetAllContentsRequest : IRequest<ContentsPagedListDTO>
    {
        public string Title { get; set; }
        public int LastId { get; set; }
        public int PageSize { get; set; }
        public string Direction { get; set; }
    }
}