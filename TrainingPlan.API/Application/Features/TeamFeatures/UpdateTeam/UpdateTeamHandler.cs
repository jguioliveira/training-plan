using FluentValidation;
using MediatR;
using TrainingPlan.API.Application.Common.Commands;
using TrainingPlan.API.Application.Features.TeamFeatures.CreateTeam;
using TrainingPlan.Domain.Repositories;

namespace TrainingPlan.API.Application.Features.TeamFeatures.UpdateTeam
{
    public class UpdateTeamHandler : IRequestHandler<UpdateTeamRequest, UpdateTeamResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITeamRepository _teamRepository;
        private readonly IValidator<UpdateTeamRequest> _validator;

        public UpdateTeamHandler(
            IUnitOfWork unitOfWork,
            ITeamRepository teamRepository,
            IValidator<UpdateTeamRequest> validator)
        {
            _unitOfWork = unitOfWork;
            _teamRepository = teamRepository;
            _validator = validator;
        }

        public async Task<UpdateTeamResponse> Handle(UpdateTeamRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return new UpdateTeamResponse(false, "Validation failure", validationResult.ToDictionary());
            }

            var team = await _teamRepository.GetAsync(request.Id, cancellationToken);

            if (team == null)
                return new UpdateTeamResponse(false, "Team was not found.");

            if (!string.IsNullOrEmpty(request.Name))
                team.UpdateName(request.Name);

            if (!string.IsNullOrEmpty(request.Email))
                team.UpdateEmail(request.Email);

            _teamRepository.Update(team);
            await _unitOfWork.Save(cancellationToken);

            return new UpdateTeamResponse(true, "Team successfully updated.");
        }
    }

    public record UpdateTeamRequest : IRequest<UpdateTeamResponse>
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public string? Email { get; set; }
    }

    public record UpdateTeamResponse : CommandResult
    {
        public UpdateTeamResponse(bool success, object message, IDictionary<string, string[]>? errors = null) : base(success, message, errors)
        {
        }
    }

    public class UpdateTeamValidator : AbstractValidator<UpdateTeamRequest>
    {
        public UpdateTeamValidator()
        {
            RuleFor(x => x.Email).MaximumLength(50).EmailAddress();
            RuleFor(x => x.Name).MinimumLength(3).MaximumLength(50);
        }
    }
}
