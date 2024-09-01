using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Entities;

namespace TrainingPlan.Domain.Repositories
{
    public interface IWorkoutRepository : IBaseRepository<Workout>
    {
        Task<WorkoutDTO?> GetWorkoutAsync(int id);
    }
}
