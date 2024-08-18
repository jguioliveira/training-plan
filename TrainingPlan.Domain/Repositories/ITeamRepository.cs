using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Entities;

namespace TrainingPlan.Domain.Repositories
{
    public interface ITeamRepository : IBaseRepository<Team>
    {

        Task<TeamDTO?> GetTeamAsync(int id);

        Task<TeamDTO?> GetTeamAsync(string email);
    }
}
