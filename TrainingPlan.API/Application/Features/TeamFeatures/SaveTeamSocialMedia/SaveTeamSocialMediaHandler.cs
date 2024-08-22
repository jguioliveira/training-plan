using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrainingPlan.API.Application.Common.Commands;
using TrainingPlan.Domain.Repositories;

namespace TrainingPlan.API.Application.Features.TeamFeatures.SaveTeamSocialMedia
{
    public class SaveTeamSocialMediaHandler : IRequestHandler<SaveTeamSocialMediaRequest, SaveTeamSocialMediaResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITeamRepository _teamRepository;
        private readonly IValidator<SaveTeamSocialMediaRequest> _validator;

        public SaveTeamSocialMediaHandler(IUnitOfWork unitOfWork,
            ITeamRepository teamRepository,
            IValidator<SaveTeamSocialMediaRequest> validator)
        {
            _unitOfWork = unitOfWork;
            _teamRepository = teamRepository;
            _validator = validator;
        }

        public async Task<SaveTeamSocialMediaResponse> Handle(SaveTeamSocialMediaRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return new SaveTeamSocialMediaResponse(false, "Validation failure", validationResult.ToDictionary());
            }

            var team = await _teamRepository.GetAsync(request.Id, cancellationToken);

            if (team == null)
                return new SaveTeamSocialMediaResponse(false, "Team was not found.");

            team.SaveSocialMedia(request.SocialMedia.Name, request.SocialMedia.Account);

            _teamRepository.Update(team);
            await _unitOfWork.Save(cancellationToken);

            return new SaveTeamSocialMediaResponse(true, "Team social media was saved.");
        }
    }

    public record SaveTeamSocialMediaRequest : IRequest<SaveTeamSocialMediaResponse>
    {
        [FromRoute]
        public int Id { get; set; }

        [FromBody]
        public SaveSocialMediaRequest SocialMedia { get; set; }

        public record SaveSocialMediaRequest
        {
            public string Name { get; set; }
            
            public string Account { get; set; }
        }

    }

    public record SaveTeamSocialMediaResponse : CommandResult
    {
        public SaveTeamSocialMediaResponse(bool success, object message, IDictionary<string, string[]>? errors = null) : base(success, message, errors)
        {
        }
    }

    public class SaveTeamSocialMediasValidator : AbstractValidator<SaveTeamSocialMediaRequest>
    {
        public SaveTeamSocialMediasValidator()
        {
            RuleFor(x => x.SocialMedia.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.SocialMedia.Account).NotEmpty().MinimumLength(3).MaximumLength(50);
        }
    }
}
