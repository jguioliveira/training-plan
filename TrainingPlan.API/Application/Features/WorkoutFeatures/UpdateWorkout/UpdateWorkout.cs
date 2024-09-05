using FluentValidation;
using MediatR;
using TrainingPlan.API.Application.Common.Commands;
using TrainingPlan.Domain.Repositories;

namespace TrainingPlan.API.Application.Features.WorkoutFeatures.UpdateWorkout
{
    public class UpdateWorkout(IValidator<UpdateWorkoutRequest> validator, IUnitOfWork unitOfWork, IContentRepository contentRepository, IWorkoutRepository workoutRepository) : IRequestHandler<UpdateWorkoutRequest, UpdateWorkoutResponse>
    {
        private readonly IValidator<UpdateWorkoutRequest> _validator = validator;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IContentRepository _contentRepository = contentRepository;
        private readonly IWorkoutRepository _workoutRepository = workoutRepository;

        public async Task<UpdateWorkoutResponse> Handle(UpdateWorkoutRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return new UpdateWorkoutResponse(false, "Validation failure", validationResult.ToDictionary());
            }

            var workout = await _workoutRepository.GetAsync(request.Id, cancellationToken);

            if (workout == null || workout.Id == 0)
                return new UpdateWorkoutResponse(false, "Workout was not found.");

            workout.UpdateDate(request.Date);
            workout.UpdateDescription(request.Description);

            if (request.ContentId > 0 && request.ContentId != workout.ContentId)
            {
                var content = await _contentRepository.GetAsync(request.ContentId.Value, cancellationToken);

                if (content == null)
                    return new UpdateWorkoutResponse(false, "Content was not found.");

                workout.UpdateContent(content);
            }

            _workoutRepository.Update(workout);
            await _unitOfWork.Save(cancellationToken);

            return new UpdateWorkoutResponse(true, "Workout successfully saved.");
        }
    }

    public record UpdateWorkoutRequest : IRequest<UpdateWorkoutResponse>
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int? ContentId { get; set; }
    }

    public record UpdateWorkoutResponse : CommandResult
    {
        public UpdateWorkoutResponse(bool success, object message, IDictionary<string, string[]>? errors = null) : base(success, message, errors)
        {
        }
    }

    public class UpdateWorkoutValidator : AbstractValidator<UpdateWorkoutRequest>
    {
        public UpdateWorkoutValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Date).NotEmpty().GreaterThanOrEqualTo(DateTime.UtcNow);
            RuleFor(x => x.Description).MaximumLength(300);
            RuleFor(x => x.ContentId).GreaterThan(0);
        }
    }
}
