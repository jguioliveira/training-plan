using MediatR;
using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Repositories;

namespace TrainingPlan.API.Application.Features.PlanFeatures.GetPlan
{
    public class GetAllPlansHandler(IPlanRepository planRepository) : IRequestHandler<GetAllPlansRequest, PlansPagedListDTO>
    {
        private readonly IPlanRepository _planRepository = planRepository;

        public Task<PlansPagedListDTO> Handle(GetAllPlansRequest request, CancellationToken cancellationToken)
        {
            return _planRepository.GetPlansAsync(request.LastId, request.PageSize, request.Direction);
        }
    }

    public record GetAllPlansRequest : IRequest<PlansPagedListDTO>
    {
        public int LastId { get; set; }
        public int PageSize { get; set; }
        public string Direction { get; set; }
    }
}
