using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Entities;

namespace TrainingPlan.Domain.Repositories
{
    public interface IPlanRepository : IBaseRepository<Plan>
    {
        Task<PlansPagedListDTO> GetPlansAsync(int lastId, int pageSize, string direction);
        Task<PlanDTO?> GetPlanAsync(int idAthlete);
    }
}
