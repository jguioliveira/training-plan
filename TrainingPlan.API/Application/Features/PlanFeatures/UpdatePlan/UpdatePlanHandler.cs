using FluentValidation;
using MediatR;
using TrainingPlan.API.Application.Common.Commands;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;

namespace TrainingPlan.API.Application.Features.PlanFeatures.UpdatePlan
{
    public class UpdatePlanHandler : IRequestHandler<UpdatePlanRequest, UpdatePlanResponse>
    {
        private readonly IValidator<UpdatePlanRequest> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPlanRepository _planRepository;
        private readonly IPersonRepository _personRepository;

        public UpdatePlanHandler(IValidator<UpdatePlanRequest> validator, IUnitOfWork unitOfWork, IPlanRepository planRepository, IPersonRepository personRepository)
        {
            _validator = validator;
            _unitOfWork = unitOfWork;
            _planRepository = planRepository;
            _personRepository = personRepository;
        }

        public async Task<UpdatePlanResponse> Handle(UpdatePlanRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return new UpdatePlanResponse(false, "Validation failure", validationResult.ToDictionary());
            }

            var plan = await _planRepository.GetAsync(request.Id, cancellationToken);

            if (plan == null || plan.Id == 0)
                return new UpdatePlanResponse(false, "Plan was not found.");

            if (request.InstructorId != 0 && request.InstructorId != plan.InstructorId)
            {
                var instructor = await _personRepository.GetAsync(request.InstructorId, cancellationToken);

                if (instructor == null || instructor.Id == 0)
                    return new UpdatePlanResponse(false, "Instructor is not valid.");

                plan.UpdateInstructor((Instructor)instructor);
            }    

            plan.UpdateName(request.Name);
            plan.UpdateDescription(request.Description);
            plan.UpdateGoal(request.Goal);

            _planRepository.Update(plan);
            await _unitOfWork.Save(cancellationToken);

            return new UpdatePlanResponse(true, "Plan successfully created.");
        }
    }

    public record UpdatePlanRequest : IRequest<UpdatePlanResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Goal { get; set; }
        public int InstructorId { get; set; }
    }

    public record UpdatePlanResponse : CommandResult
    {
        public UpdatePlanResponse(bool success, object message, IDictionary<string, string[]>? errors = null) : base(success, message, errors)
        {
        }
    }

    public class UpdatePlanValidator : AbstractValidator<UpdatePlanRequest>
    {
        public UpdatePlanValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(50);
            RuleFor(x => x.Goal).NotEmpty().MinimumLength(3).MaximumLength(50);
            RuleFor(x => x.Description).MaximumLength(100);
            RuleFor(x => x.InstructorId).GreaterThan(0);            
        }
    }
}