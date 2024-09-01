using MediatR;
using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Repositories;

namespace TrainingPlan.API.Application.Features.PlanFeatures.GetPlan
{
    public class GetPlanHandler(IPlanRepository planRepository) : IRequestHandler<GetPlanRequest, PlanDTO?>
    {
        private readonly IPlanRepository _planRepository = planRepository;

        public Task<PlanDTO?> Handle(GetPlanRequest request, CancellationToken cancellationToken)
        {
            return _planRepository.GetPlanAsync(request.AthleteId);
        }
    }

    public record GetPlanRequest : IRequest<PlanDTO>
    {
        //int rowId, int pageSize,
        public int AthleteId { get; set; }
    }
}
