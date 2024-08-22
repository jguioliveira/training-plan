using Microsoft.EntityFrameworkCore;
using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;
using TrainingPlan.Infrastructure.DbContext;

namespace TrainingPlan.Infrastructure.Repositories
{
    public class TeamRepository(ApplicationDbContext dbContext) : BaseRepository<Team>(dbContext), ITeamRepository
    {
        public override Task<Team?> GetAsync(int id, CancellationToken cancellationToken)
        {
            return dbContext.Teams
                .Include(t => t.TeamSettings)
                .Include(t => t.SocialsMedia)
                .SingleOrDefaultAsync(t => t.Id == id);
        }
        public async Task<TeamDTO?> GetTeamAsync(int id)
        {
            string query = "SELECT * FROM \"Teams\" WHERE \"Id\" = @param;";
            query += "\nSELECT * FROM \"TeamSettings\" WHERE \"TeamId\" = @param;";
            query += "\nSELECT * FROM \"SocialMedia\" WHERE \"TeamId\" = @param;";

            var result = await GetWithParentsAsync(query, id);

            var team = await result.ReadFirstAsync<TeamDTO>();
            team.TeamSettings = await result.ReadAsync<TeamSettingsDTO>();
            team.SocialsMedia = await result.ReadAsync<SocialMediaDTO>();

            return team;
        }

        public Task<TeamDTO?> GetTeamAsync(string email)
        {
            string query = "SELECT * FROM \"Teams\" WHERE \"Email\" = @param;";

            return GetAsync<TeamDTO?>(query, email);
        }
    }
}
