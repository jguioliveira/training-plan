using MediatR;
using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Repositories;

namespace TrainingPlan.API.Application.Features.WorkoutFeatures.GetWorkout
{
    public class GetWorkoutHandler(IWorkoutRepository workoutRepository) : IRequestHandler<GetWorkoutRequest, WorkoutDTO?>
    {
        private readonly IWorkoutRepository _workoutRepository = workoutRepository;

        public Task<WorkoutDTO?> Handle(GetWorkoutRequest request, CancellationToken cancellationToken)
        {
            return _workoutRepository.GetWorkoutAsync(request.Id);
        }
    }

    public record GetWorkoutRequest : IRequest<WorkoutDTO>
    {
        public int Id { get; set; }
    }
}
