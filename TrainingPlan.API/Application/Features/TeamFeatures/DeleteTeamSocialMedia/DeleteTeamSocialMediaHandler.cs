using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrainingPlan.API.Application.Common.Commands;
using TrainingPlan.Domain.Repositories;

namespace TrainingPlan.API.Application.Features.TeamFeatures.DeleteTeamSocialMedia
{
    public class DeleteTeamSocialMediaHandler : IRequestHandler<DeleteTeamSocialMediaRequest, DeleteTeamSocialMediaResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITeamRepository _teamRepository;

        public DeleteTeamSocialMediaHandler(IUnitOfWork unitOfWork,
            ITeamRepository teamRepository)
        {
            _unitOfWork = unitOfWork;
            _teamRepository = teamRepository;
        }

        public async Task<DeleteTeamSocialMediaResponse> Handle(DeleteTeamSocialMediaRequest request, CancellationToken cancellationToken)
        {
            var team = await _teamRepository.GetAsync(request.Id, cancellationToken);

            if (team == null)
                return new DeleteTeamSocialMediaResponse(false, "Team was not found.");

            team.DeleteSocialMedia(request.SocialMedia.Name);

            _teamRepository.Update(team);
            await _unitOfWork.Save(cancellationToken);

            return new DeleteTeamSocialMediaResponse(true, "Team social media was deleted.");
        }
    }

    public record DeleteTeamSocialMediaRequest : IRequest<DeleteTeamSocialMediaResponse>
    {
        [FromRoute]
        public int Id { get; set; }

        [FromBody]
        public DeleteSocialMediaRequest SocialMedia { get; set; }

        public record DeleteSocialMediaRequest
        {
            public string Name { get; set; }
        }
    }

    public record DeleteTeamSocialMediaResponse : CommandResult
    {
        public DeleteTeamSocialMediaResponse(bool success, object message, IDictionary<string, string[]>? errors = null) : base(success, message, errors)
        {
        }
    }
}
