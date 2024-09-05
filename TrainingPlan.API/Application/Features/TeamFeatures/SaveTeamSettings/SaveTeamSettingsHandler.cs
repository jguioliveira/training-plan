using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrainingPlan.API.Application.Common.Commands;
using TrainingPlan.Domain.Repositories;

namespace TrainingPlan.API.Application.Features.TeamFeatures.SaveTeamSettings
{
    public class SaveTeamSettingsHandler : IRequestHandler<SaveTeamSettingsRequest, SaveTeamSettingsResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITeamRepository _teamRepository;
        private readonly IValidator<SaveTeamSettingsRequest> _validator;

        public SaveTeamSettingsHandler(IUnitOfWork unitOfWork,
            ITeamRepository teamRepository,
            IValidator<SaveTeamSettingsRequest> validator)
        {
            _unitOfWork = unitOfWork;
            _teamRepository = teamRepository;
            _validator = validator;
        }


        public async Task<SaveTeamSettingsResponse> Handle(SaveTeamSettingsRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return new SaveTeamSettingsResponse(false, "Validation failure", validationResult.ToDictionary());
            }

            var team = await _teamRepository.GetAsync(request.teamId, cancellationToken);

            if (team == null)
                return new SaveTeamSettingsResponse(false, "Team was not found.");

            foreach (var item in request.Settings)
            {
                team.AddSettings(item.Key, item.Value);
            }

            _teamRepository.Update(team);
            await _unitOfWork.Save(cancellationToken);

            return new SaveTeamSettingsResponse(true, "Team settings were saved.");
        }
    }

    public record SaveTeamSettingsRequest : IRequest<SaveTeamSettingsResponse>
    {
        [FromRoute]
        public int teamId { get; set; }

        [FromBody]
        public Dictionary<string, string> Settings { get; set; }
    }

    public record SaveTeamSettingsResponse : CommandResult
    {
        public SaveTeamSettingsResponse(bool success, object message, IDictionary<string, string[]>? errors = null) : base(success, message, errors)
        {
        }
    }

    public class SaveTeamSettingsValidator : AbstractValidator<SaveTeamSettingsRequest>
    {
        public SaveTeamSettingsValidator()
        {
            RuleFor(x => x.Settings).Custom((list, context) => {
                if (list.Any(l => string.IsNullOrEmpty(l.Key) || string.IsNullOrEmpty(l.Value) ))
                {
                    context.AddFailure("Settings Key and value are required.");
                }
            });
        }
    }
}
