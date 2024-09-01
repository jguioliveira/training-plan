using FluentValidation;
using MediatR;
using TrainingPlan.API.Application.Common.Commands;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;

namespace TrainingPlan.API.Application.Features.PlanFeatures.CreatePlan
{
    public class CreatePlanHandler : IRequestHandler<CreatePlanRequest, CreatePlanResponse>
    {
        private readonly IValidator<CreatePlanRequest> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPlanRepository _planRepository;
        private readonly IPersonRepository _personRepository;

        public CreatePlanHandler(IValidator<CreatePlanRequest> validator, IUnitOfWork unitOfWork, IPlanRepository planRepository, IPersonRepository personRepository)
        {
            _validator = validator;
            _unitOfWork = unitOfWork;
            _planRepository = planRepository;
            _personRepository = personRepository;
        }

        public async Task<CreatePlanResponse> Handle(CreatePlanRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return new CreatePlanResponse(false, "Validation failure", validationResult.ToDictionary());
            }

            var athlete = await _personRepository.GetAsync(request.AhtleteId, cancellationToken);
            var instructor = await _personRepository.GetAsync(request.InstructorId, cancellationToken);

            if (athlete == null || athlete.Id == 0)
                return new CreatePlanResponse(false, "Athlete is not valid.");

            if (instructor == null || instructor.Id == 0)
                return new CreatePlanResponse(false, "Instructor is not valid.");

            var plan = new Plan(request.Name, request.Goal, request.AhtleteId, request.InstructorId, request.Description);            

            _planRepository.Create(plan);
            await _unitOfWork.Save(cancellationToken);

            return new CreatePlanResponse(true, "Plan successfully created.");
        }
    }

    public record CreatePlanRequest : IRequest<CreatePlanResponse>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Goal { get; set; }
        public int AhtleteId { get; set; }
        public int InstructorId { get; set; }
    }

    public record CreatePlanResponse : CommandResult
    {
        public CreatePlanResponse(bool success, object message, IDictionary<string, string[]>? errors = null) : base(success, message, errors)
        {
        }
    }

    public class CreatePlanValidator : AbstractValidator<CreatePlanRequest>
    {
        public CreatePlanValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(50);
            RuleFor(x => x.Goal).NotEmpty().MinimumLength(3).MaximumLength(50);
            RuleFor(x => x.Description).MaximumLength(100);
            RuleFor(x => x.AhtleteId).GreaterThan(0);
            RuleFor(x => x.InstructorId).GreaterThan(0);
        }
    }

}
