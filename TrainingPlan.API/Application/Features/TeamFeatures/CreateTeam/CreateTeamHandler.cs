using FluentValidation;
using MediatR;
using TrainingPlan.API.Application.Common.Commands;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;

namespace TrainingPlan.API.Application.Features.TeamFeatures.CreateTeam
{
    public class CreateTeamHandler : IRequestHandler<CreateTeamRequest, CreateTeamResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITeamRepository _teamRepository;
        private readonly IValidator<CreateTeamRequest> _validator;

        public CreateTeamHandler(
            IUnitOfWork unitOfWork,
            ITeamRepository teamRepository,
            IValidator<CreateTeamRequest> validator)
        {
            _unitOfWork = unitOfWork;
            _teamRepository = teamRepository;
            _validator = validator;
        }

        public async Task<CreateTeamResponse> Handle(CreateTeamRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return new CreateTeamResponse(false, "Validation failure", validationResult.ToDictionary());
            }

            var result = await _teamRepository.GetTeamAsync(request.Email);

            if(result != null)
            {
                return new CreateTeamResponse(false, "E-mail already registered.");
            }

            var team = new Team(request.Name, request.Email);

            _teamRepository.Create(team);
            await _unitOfWork.Save(cancellationToken);

            return new CreateTeamResponse(true, "Team successfully created.");
        }
    }

    public record CreateTeamRequest : IRequest<CreateTeamResponse>
    {
        public string Name { get; set; }

        public string Email { get; set; }
    }

    public record CreateTeamResponse : CommandResult
    {
        public CreateTeamResponse(bool success, object message, IDictionary<string, string[]>? errors = null) : base(success, message, errors)
        {
        }
    }

    public class CreateTeamValidator : AbstractValidator<CreateTeamRequest>
    {
        public CreateTeamValidator()
        {
            RuleFor(x => x.Email).NotEmpty().MaximumLength(50).EmailAddress();
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(50);
        }
    }
}
