using FluentValidation;
using MediatR;
using TrainingPlan.API.Application.Common.Commands;
using TrainingPlan.Domain.Repositories;

namespace TrainingPlan.API.Application.Features.InstructorFeatures.UpdateInstructor
{
    public class UpdateInstructorHandler : IRequestHandler<UpdateInstructorRequest, UpdateInstructorResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPersonRepository _instructorRepository;
        private readonly IValidator<UpdateInstructorRequest> _validator;

        public UpdateInstructorHandler(
            IUnitOfWork unitOfWork,
            IPersonRepository instructorRepository,
            IValidator<UpdateInstructorRequest> validator)
        {
            _unitOfWork = unitOfWork;
            _instructorRepository = instructorRepository;
            _validator = validator;
        }

        public async Task<UpdateInstructorResponse> Handle(UpdateInstructorRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return new UpdateInstructorResponse(false, "Validation failure", validationResult.ToDictionary());
            }

            var instructor = await _instructorRepository.GetAsync(request.Id, cancellationToken);

            if (instructor is null)
                return new UpdateInstructorResponse(false, "Instructor was not found.");

            if (!string.IsNullOrEmpty(request.Name))
                instructor.UpdateName(request.Name);

            if (!string.IsNullOrEmpty(request.Phone))
                instructor.UpdatePhone(request.Phone);

            if (request.Birth != null && request.Birth != DateTime.MinValue)
                instructor.UpdateBirth(request.Birth.Value);

            _instructorRepository.Update(instructor);

            await _unitOfWork.Save(cancellationToken);

            return new UpdateInstructorResponse(true, "Instructor successfully updated.");
        }
    }

    public class UpdateInstructorRequest : IRequest<UpdateInstructorResponse>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? Birth { get; set; }
        public string? Phone { get; set; }
    }

    public class UpdateInstructorValidator : AbstractValidator<UpdateInstructorRequest>
    {
        public UpdateInstructorValidator()
        {
            RuleFor(x => x.Name).MinimumLength(3).MaximumLength(50);
        }
    }

    public sealed record UpdateInstructorResponse : CommandResult
    {
        public UpdateInstructorResponse(bool success, object message, IDictionary<string, string[]>? errors = null) : base(success, message, errors)
        {
        }
    }
}
