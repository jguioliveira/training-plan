using Microsoft.EntityFrameworkCore;
using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;
using TrainingPlan.Infrastructure.DbContext;

namespace TrainingPlan.Infrastructure.Repositories
{
    public class PlanRepository(ApplicationDbContext dbContext) : BaseRepository<Plan>(dbContext), IPlanRepository
    {
        public override Task<Plan?> GetAsync(int id, CancellationToken cancellationToken)
        {
            return dbContext.Plans
                //.Include(t => t.Athlete)
                //.Include(t => t.Instructor)
                .SingleOrDefaultAsync(t => t.Id == id);
        }

        public async Task<PlanDTO?> GetPlanAsync(int id)
        {
            string query = "SELECT * FROM \"Plans\" WHERE \"AthleteId\" = @param;";

            var plan = await GetAsync<PlanDTO?>(query, id);

            if(plan == null)
                return null;

            query = "\nSELECT * FROM \"Workouts\" WHERE \"PlanId\" = @planId;";
            query += "\nSELECT * FROM \"Person\" WHERE \"Id\" = @instructorId;";

            var dictionary = new Dictionary<string, object>
            {
                { "planId", plan.Id },
                { "instructorId", plan.InstructorId }
            };

            var result = await GetWithParentsAsync(query, dictionary);

            plan.Workouts = await result.ReadAsync<WorkoutDTO>();
            plan.Instructor = await result.ReadSingleAsync<InstructorDTO>();

            return plan;
        }
    }
}
