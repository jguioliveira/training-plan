using MediatR;
using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Repositories;

namespace TrainingPlan.API.Application.Features.InstructorFeatures.GetInstructor
{
    public class GetInstructorHandler : IRequestHandler<GetInstructorRequest, InstructorDTO?>
    {
        private readonly IPersonRepository _instructorRepository;

        public GetInstructorHandler(IPersonRepository instructorRepository)
        {
            _instructorRepository = instructorRepository;
        }

        public Task<InstructorDTO?> Handle(GetInstructorRequest request, CancellationToken cancellationToken)
        {
            return _instructorRepository.GetInstructorAsync(request.Id);
        }
    }

    public record GetInstructorRequest : IRequest<InstructorDTO?>
    {
        public int Id { get; set; }
    }
}
