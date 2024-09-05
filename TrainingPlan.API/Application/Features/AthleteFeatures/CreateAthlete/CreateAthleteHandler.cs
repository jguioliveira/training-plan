using FluentValidation;
using MediatR;
using TrainingPlan.API.Application.Common.Commands;
using TrainingPlan.API.Application.Common.Services;

namespace TrainingPlan.API.Application.Features.AthleteFeatures.CreateAthlete
{
    public sealed class CreateAthleteHandler : IRequestHandler<CreateAthleteRequest, CreateAthleteResponse>
    {
        private readonly IValidator<CreateAthleteRequest> _validator;
        private readonly IUserService _userService;

        public CreateAthleteHandler(
            IValidator<CreateAthleteRequest> validator,
            IUserService userService
            )
        {
            _validator = validator;
            _userService = userService;
        }

        public async Task<CreateAthleteResponse> Handle(CreateAthleteRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return new CreateAthleteResponse(false, "Validation failure", validationResult.ToDictionary());
            }

            await _userService.CreateAthleteAsync(request.Name, request.Email, request.Password, request.Birth, request.Phone, cancellationToken);

            return new CreateAthleteResponse(true, "Athlete successfully created.");
        }
    }

    public sealed record CreateAthleteRequest(string Email, string Password, string Name, DateTime Birth, string Phone) : IRequest<CreateAthleteResponse>
    {
    }

    public sealed record CreateAthleteResponse : CommandResult
    {
        public CreateAthleteResponse(bool success, object message, IDictionary<string, string[]>? errors = null) : base(success, message, errors)
        {
        }
    }

    public class CreateAthleteValidator : AbstractValidator<CreateAthleteRequest>
    {
        public CreateAthleteValidator()
        {
            RuleFor(x => x.Email).NotEmpty().MaximumLength(50).EmailAddress();
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(50);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(20);
            RuleFor(x => x.Birth).NotEmpty();
        }
    }

}
