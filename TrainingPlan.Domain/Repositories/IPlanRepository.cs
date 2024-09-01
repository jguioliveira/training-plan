using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Entities;

namespace TrainingPlan.Domain.Repositories
{
    public interface IPlanRepository : IBaseRepository<Plan>
    {
        Task<PlanDTO?> GetPlanAsync(int idAthlete);
    }
}
