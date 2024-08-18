using MediatR;
using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Repositories;

namespace TrainingPlan.API.Application.Features.TeamFeatures.GetTeam
{
    public class GetTeamHandler : IRequestHandler<GetTeamRequest, TeamDTO>
    {
        private readonly ITeamRepository _teamRepository;

        public GetTeamHandler(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public Task<TeamDTO> Handle(GetTeamRequest request, CancellationToken cancellationToken)
        {
            return _teamRepository.GetTeamAsync(request.Id);
        }
    }

    public record GetTeamRequest : IRequest<TeamDTO>
    {
        //int rowId, int pageSize,
        public int Id { get; set; }
    }
}
