using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using TrainingPlan.API.Application.Common.Commands;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;

namespace TrainingPlan.API.Application.Features.WorkoutFeatures.CreateComment
{
    public class CreateCommentHandler : IRequestHandler<CreateWorkoutCommentRequest, CreateCommentResponse>
    {
        private readonly IValidator<CreateWorkoutCommentRequest> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IPersonRepository _personRepository;

        public CreateCommentHandler(IValidator<CreateWorkoutCommentRequest> validator, IUnitOfWork unitOfWork, IWorkoutRepository workoutRepository, IPersonRepository personRepository)
        {
            _validator = validator;
            _unitOfWork = unitOfWork;
            _workoutRepository = workoutRepository;
            _personRepository = personRepository;
        }

        public async Task<CreateCommentResponse> Handle(CreateWorkoutCommentRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return new CreateCommentResponse(false, "Validation failure", validationResult.ToDictionary());
            }

            var person = await _personRepository.GetAsync(request.PersonId, cancellationToken);

            if (person == null || person.Id == 0)
                return new CreateCommentResponse(false, "Person is not valid.");

            var workout = await _workoutRepository.GetAsync(request.WorkoutId, cancellationToken);

            if (workout == null || workout.Id == 0)
                return new CreateCommentResponse(false, "Workout is not valid.");

            var comment = new Comment(person.Id, person.Name, person.Type, request.Text, request.WorkoutId);

            workout.AddComment(comment);

            _workoutRepository.Update(workout);
            await _unitOfWork.Save(cancellationToken);

            return new CreateCommentResponse(true, "Comment successfully created.");
        }
    }

    public class CreateWorkoutCommentRequest : IRequest<CreateCommentResponse>
    {
        [JsonIgnore]
        public int WorkoutId { get; set; }

        public int PersonId { get; set; }

        public string Text { get; set; }
    }

    

    public record CreateCommentResponse : CommandResult
    {
        public CreateCommentResponse(bool success, object message, IDictionary<string, string[]>? errors = null) : base(success, message, errors)
        {
        }
    }

    public class CreateCommentValidator : AbstractValidator<CreateWorkoutCommentRequest>
    {
        public CreateCommentValidator()
        {
            RuleFor(x => x.WorkoutId).GreaterThan(0);
            RuleFor(x => x.PersonId).GreaterThan(0);
            RuleFor(x => x.Text).NotEmpty().MinimumLength(3).MaximumLength(300);
            
        }
    }

}
