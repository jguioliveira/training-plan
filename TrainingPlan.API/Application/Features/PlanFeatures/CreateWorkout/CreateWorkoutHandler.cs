using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrainingPlan.API.Application.Common.Commands;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;

namespace TrainingPlan.API.Application.Features.PlanFeatures.CreateWorkout
{
    public class CreateWorkoutHandler(IValidator<CreatePlanWorkoutRequest> validator, IUnitOfWork unitOfWork, IPlanRepository planRepository, IWorkoutRepository workoutRepository) : IRequestHandler<CreatePlanWorkoutRequest, CreateWorkoutResponse>
    {
        private readonly IValidator<CreatePlanWorkoutRequest> _validator = validator;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPlanRepository _planRepository = planRepository;
        private readonly IWorkoutRepository _workoutRepository = workoutRepository;

        public async Task<CreateWorkoutResponse> Handle(CreatePlanWorkoutRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return new CreateWorkoutResponse(false, "Validation failure", validationResult.ToDictionary());
            }

            var plan = await _planRepository.GetAsync(request.Id, cancellationToken);

            if (plan == null || plan.Id == 0)
                return new CreateWorkoutResponse(false, "Plan was not found.");

            var workout = new Workout(request.Workout.Date, request.Workout.Description, request.Id, request.Workout.ContentId);

            _workoutRepository.Create(workout);
            await _unitOfWork.Save(cancellationToken);

            return new CreateWorkoutResponse(true, "Workout successfully saved.");
        }
    }

    public record CreatePlanWorkoutRequest : IRequest<CreateWorkoutResponse>
    {
        [FromRoute]
        public int Id { get; set; }

        [FromBody]
        public CreateWorkoutRequest Workout { get; set; }

        public record CreateWorkoutRequest : IRequest<CreateWorkoutResponse>
        {
            public DateTime Date { get; set; }
            public string Description { get; set; }
            public int? ContentId { get; set; }
        }
    }

    

    public record CreateWorkoutResponse : CommandResult
    {
        public CreateWorkoutResponse(bool success, object message, IDictionary<string, string[]>? errors = null) : base(success, message, errors)
        {
        }
    }

    public class CreateWorkoutValidator : AbstractValidator<CreatePlanWorkoutRequest>
    {
        public CreateWorkoutValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Workout).NotNull();
            RuleFor(x => x.Workout.Date).NotEmpty().GreaterThanOrEqualTo(DateTime.UtcNow);
            RuleFor(x => x.Workout.Description).MaximumLength(300);
            RuleFor(x => x.Workout.ContentId).GreaterThan(0);
        }
    }
}
