using FluentValidation;
using MediatR;
using TrainingPlan.API.Application.Common.Commands;

namespace TrainingPlan.API.Application.Features.TeamFeatures.SaveTeamSettings
{
    public class SaveTeamSettingsHandler : IRequestHandler<SaveTeamSettingsRequest, SaveTeamSettingsResponse>
    {
        public Task<SaveTeamSettingsResponse> Handle(SaveTeamSettingsRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public record SaveTeamSettingsRequest : IRequest<SaveTeamSettingsResponse>
    {
        public int TeamId { get; set; }

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
            
        }
    }
}
