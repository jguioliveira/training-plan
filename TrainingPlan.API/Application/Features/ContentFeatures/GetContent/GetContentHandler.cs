using MediatR;
using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Repositories;

namespace TrainingPlan.API.Application.Features.ContentFeatures.GetContent
{
    public class GetContentHandler(IContentRepository contentRepository) : IRequestHandler<GetContentRequest, ContentDTO?>
    {
        private readonly IContentRepository _contentRepository = contentRepository;

        public Task<ContentDTO?> Handle(GetContentRequest request, CancellationToken cancellationToken)
        {
            return _contentRepository.GetContentAsync(request.Id);
        }
    }

    public record GetContentRequest : IRequest<ContentDTO>
    {
        public int Id { get; set; }
    }
}
