using MediatR;
using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Repositories;

namespace TrainingPlan.API.Application.Features.PlanFeatures.GetWorkout
{
    public class GetWorkoutHandler(IWorkoutRepository workoutRepository) : IRequestHandler<GetWorkoutRequest, WorkoutDTO?>
    {
        private readonly IWorkoutRepository _workoutRepository = workoutRepository;

    public Task<WorkoutDTO?> Handle(GetWorkoutRequest request, CancellationToken cancellationToken)
    {
        return _workoutRepository.GetWorkoutAsync(request.IdWorkout);
    }
}

public record GetWorkoutRequest : IRequest<WorkoutDTO>
{
    public int IdWorkout { get; set; }
}
}
