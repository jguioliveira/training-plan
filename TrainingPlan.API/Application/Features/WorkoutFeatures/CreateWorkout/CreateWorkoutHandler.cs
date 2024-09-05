using FluentValidation;
using MediatR;
using TrainingPlan.API.Application.Common.Commands;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;

namespace TrainingPlan.API.Application.Features.WorkoutFeatures.CreateWorkout
{
    public class CreateWorkoutHandler(IValidator<CreateWorkoutRequest> validator, IUnitOfWork unitOfWork, IPlanRepository planRepository, IWorkoutRepository workoutRepository) : IRequestHandler<CreateWorkoutRequest, CreateWorkoutResponse>
    {
        private readonly IValidator<CreateWorkoutRequest> _validator = validator;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPlanRepository _planRepository = planRepository;
        private readonly IWorkoutRepository _workoutRepository = workoutRepository;

        public async Task<CreateWorkoutResponse> Handle(CreateWorkoutRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return new CreateWorkoutResponse(false, "Validation failure", validationResult.ToDictionary());
            }

            var plan = await _planRepository.GetAsync(request.PlanId, cancellationToken);

            if (plan == null || plan.Id == 0)
                return new CreateWorkoutResponse(false, "Plan was not found.");

            var workout = new Workout(request.Date, request.Description, request.PlanId, request.ContentId);

            _workoutRepository.Create(workout);
            await _unitOfWork.Save(cancellationToken);

            return new CreateWorkoutResponse(true, "Workout successfully saved.");
        }
    }

    public record CreateWorkoutRequest : IRequest<CreateWorkoutResponse>
    {
        public int PlanId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int? ContentId { get; set; }
    }



    public record CreateWorkoutResponse : CommandResult
    {
        public CreateWorkoutResponse(bool success, object message, IDictionary<string, string[]>? errors = null) : base(success, message, errors)
        {
        }
    }

    public class CreateWorkoutValidator : AbstractValidator<CreateWorkoutRequest>
    {
        public CreateWorkoutValidator()
        {
            RuleFor(x => x.PlanId).GreaterThan(0);
            RuleFor(x => x.Date).NotEmpty().GreaterThanOrEqualTo(DateTime.UtcNow);
            RuleFor(x => x.Description).MaximumLength(300);
            RuleFor(x => x.ContentId).GreaterThan(0);
        }
    }
}
