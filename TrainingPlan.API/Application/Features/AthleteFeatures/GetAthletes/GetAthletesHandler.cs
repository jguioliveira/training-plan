using MediatR;
using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Repositories;

namespace TrainingPlan.API.Application.Features.AthleteFeatures.GetAthletes
{
    public class GetAthletesHandler : IRequestHandler<GetAthletesRequest, AthletesPagedListDTO>
    {
        private readonly IPersonRepository _personRepository;

        public GetAthletesHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<AthletesPagedListDTO> Handle(GetAthletesRequest request, CancellationToken cancellationToken)
        {
            var result = await _personRepository.GetAthletesAsync(request.LastId, request.PageSize, request.Direction, request.Name);

            return result;
        }
    }

    public record GetAthletesRequest : IRequest<AthletesPagedListDTO>
    {
        //int rowId, int pageSize,
        public string Name { get; set; }
        public int LastId { get; set; }
        public int PageSize { get; set; }
        public string Direction { get; set; }
    }

    public record GetAthletesResponse : AthletesPagedListDTO
    {

    }
}
