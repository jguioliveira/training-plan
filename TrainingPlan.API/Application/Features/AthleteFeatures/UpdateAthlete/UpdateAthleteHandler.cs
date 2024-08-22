using FluentValidation;
using MediatR;
using TrainingPlan.API.Application.Common.Commands;
using TrainingPlan.API.Application.Features.AthleteFeatures.CreateAthlete;
using TrainingPlan.Domain.Repositories;

namespace TrainingPlan.API.Application.Features.AthleteFeatures.UpdateAthlete
{
    public class UpdateAthleteHandler : IRequestHandler<UpdateAthleteRequest, UpdateAthleteResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPersonRepository _personRepository;
        private readonly IValidator<UpdateAthleteRequest> _validator;

        public UpdateAthleteHandler(
            IUnitOfWork unitOfWork,
            IPersonRepository personRepository,
            IValidator<UpdateAthleteRequest> validator)
        {
            _unitOfWork = unitOfWork;
            _personRepository = personRepository;
            _validator = validator;
        }

        public async Task<UpdateAthleteResponse> Handle(UpdateAthleteRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return new UpdateAthleteResponse(false, "Validation failure", validationResult.ToDictionary());
            }

            var athlete = await _personRepository.GetAsync(request.Id, cancellationToken);

            if(athlete is null)
                return new UpdateAthleteResponse(false, "Athlete was not found.");

            if (!string.IsNullOrEmpty(request.Name))
                athlete.UpdateName(request.Name);

            if (!string.IsNullOrEmpty(request.Phone))
                athlete.UpdatePhone(request.Phone);

            if (request.Birth != null && request.Birth != DateTime.MinValue)
                athlete.UpdateBirth(request.Birth.Value);

            _personRepository.Update(athlete);

            await _unitOfWork.Save(cancellationToken);

            return new UpdateAthleteResponse(true, "Athlete successfully updated.");
        }
    }

    public class UpdateAthleteRequest : IRequest<UpdateAthleteResponse>
    {
        public int Id { get; set; }
        public string? Name { get; set; } 
        public DateTime? Birth { get; set; }
        public string? Phone { get; set; }
    }

    public class UpdateAthleteValidator : AbstractValidator<UpdateAthleteRequest>
    {
        public UpdateAthleteValidator()
        {
            RuleFor(x => x.Name).MinimumLength(3).MaximumLength(50);
        }
    }

    public sealed record UpdateAthleteResponse : CommandResult
    {
        public UpdateAthleteResponse(bool success, object message, IDictionary<string, string[]>? errors = null) : base(success, message, errors)
        {
        }
    }
}
