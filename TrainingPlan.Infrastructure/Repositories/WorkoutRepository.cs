using Microsoft.EntityFrameworkCore;    
using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;
using TrainingPlan.Infrastructure.DbContext;

namespace TrainingPlan.Infrastructure.Repositories
{
    public class WorkoutRepository(ApplicationDbContext dbContext) : BaseRepository<Workout>(dbContext), IWorkoutRepository
    {
        public override Task<Workout?> GetAsync(int id, CancellationToken cancellationToken)
        {
            return dbContext.Workouts
                .SingleOrDefaultAsync(t => t.Id == id);
        }

        public async Task<WorkoutDTO?> GetWorkoutAsync(int id)
        {
            string query = "SELECT * FROM \"Workouts\" WHERE \"Id\" = @param;";            
            query += "\nSELECT * FROM \"Comments\" WHERE \"WorkoutId\" = @param;";
            
            var result = await GetWithParentsAsync(query, id);

            var workout = await result.ReadSingleAsync<WorkoutDTO>();
            workout.Comments = await result.ReadAsync<CommentDTO>();

            if(workout.ContentId != null && workout.ContentId > 0)
            {
                query = "\nSELECT * FROM \"Contents\" WHERE \"Id\" = @param;";

                var content = await GetAsync<ContentDTO?>(query, workout.ContentId);
                workout.Content = content;
            }            

            return workout;
        }
    }
}
