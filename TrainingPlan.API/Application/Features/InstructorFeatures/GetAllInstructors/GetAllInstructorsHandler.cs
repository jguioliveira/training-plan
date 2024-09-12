using MediatR;
using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Repositories;

namespace TrainingPlan.API.Application.Features.InstructorFeatures.GetAllInstructors
{
    public class GetAllInstructorsHandler : IRequestHandler<GetAllInstructorsRequest, InstructorsPagedListDTO>
    {
        private readonly IPersonRepository _instructorRepository;

        public GetAllInstructorsHandler(IPersonRepository instructorRepository)
        {
            _instructorRepository = instructorRepository;
        }

        public async Task<InstructorsPagedListDTO> Handle(GetAllInstructorsRequest request, CancellationToken cancellationToken)
        {
            var result = await _instructorRepository.GetInstructorsAsync(request.LastId, request.PageSize, request.Direction, request.Name);

            return result;
        }
    }

    public record GetAllInstructorsRequest : IRequest<InstructorsPagedListDTO>
    {
        public string Name { get; set; }
        public int LastId { get; set; }
        public int PageSize { get; set; }
        public string Direction { get; set; }
    }
}

