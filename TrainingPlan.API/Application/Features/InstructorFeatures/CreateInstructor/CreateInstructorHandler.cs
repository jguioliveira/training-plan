using FluentValidation;
using MediatR;
using TrainingPlan.API.Application.Common.Commands;
using TrainingPlan.API.Application.Common.Services;

namespace TrainingPlan.API.Application.Features.InstructorFeatures.CreateInstructor
{
    public sealed class CreateInstructorHandler : IRequestHandler<CreateInstructorRequest, CreateInstructorResponse>
    {
        private readonly IValidator<CreateInstructorRequest> _validator;
        private readonly IUserService _userService;

        public CreateInstructorHandler(
            IValidator<CreateInstructorRequest> validator,
            IUserService userService
            )
        {
            _validator = validator;
            _userService = userService;
        }

        public async Task<CreateInstructorResponse> Handle(CreateInstructorRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return new CreateInstructorResponse(false, "Validation failure", validationResult.ToDictionary());
            }

            await _userService.CreateInstructorAsync(request.Name, request.Email, request.Password, request.Birth, request.Phone, cancellationToken);

            return new CreateInstructorResponse(true, "Instructor successfully created.");
        }
    }

    public sealed record CreateInstructorRequest(string Email, string Password, string Name, DateTime Birth, string Phone) : IRequest<CreateInstructorResponse>
    {
    }

    public sealed record CreateInstructorResponse : CommandResult
    {
        public CreateInstructorResponse(bool success, object message, IDictionary<string, string[]>? errors = null) : base(success, message, errors)
        {
        }
    }

    public class CreateInstructorValidator : AbstractValidator<CreateInstructorRequest>
    {
        public CreateInstructorValidator()
        {
            RuleFor(x => x.Email).NotEmpty().MaximumLength(50).EmailAddress();
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(50);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(20);
            RuleFor(x => x.Birth).NotEmpty();
        }
    }
}
