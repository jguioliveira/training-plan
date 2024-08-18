using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;
using TrainingPlan.Infrastructure.DbContext;

namespace TrainingPlan.Infrastructure.Repositories
{
    public class TeamRepository(ApplicationDbContext dbContext) : BaseRepository<Team>(dbContext), ITeamRepository
    {
        public Task<TeamDTO?> GetTeamAsync(int id)
        {
            string query = "SELECT * FROM \"Teams\" WHERE \"Id\" = @param;";

            return GetAsync<TeamDTO?>(query, id);
        }

        public Task<TeamDTO?> GetTeamAsync(string email)
        {
            string query = "SELECT * FROM \"Teams\" WHERE \"Email\" = @param;";

            return GetAsync<TeamDTO?>(query, email);
        }
    }
}
