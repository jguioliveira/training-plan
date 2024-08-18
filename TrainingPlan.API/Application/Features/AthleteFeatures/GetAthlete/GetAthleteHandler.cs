using MediatR;
using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Repositories;

namespace TrainingPlan.API.Application.Features.AthleteFeatures.GetAthlete
{
    public class GetAthleteHandler : IRequestHandler<GetAthleteRequest, AthleteDTO?>
    {
        private readonly IPersonRepository _personRepository;

        public GetAthleteHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public Task<AthleteDTO?> Handle(GetAthleteRequest request, CancellationToken cancellationToken)
        {
            return _personRepository.GetAthleteAsync(request.Id);
        }
    }

    public record GetAthleteRequest : IRequest<AthleteDTO>
    {
        //int rowId, int pageSize,
        public int Id { get; set; }
    }
}
