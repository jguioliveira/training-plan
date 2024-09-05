using FluentValidation;
using MediatR;
using System.Text.Json.Serialization;
using TrainingPlan.API.Application.Common.Commands;
using TrainingPlan.Domain.Repositories;

namespace TrainingPlan.API.Application.Features.WorkoutFeatures.UpdateComment
{
    public class UpdateCommentHandler : IRequestHandler<UpdateCommentRequest, UpdateCommentResponse>
    {
        private readonly IValidator<UpdateCommentRequest> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWorkoutRepository _workoutRepository;

        public UpdateCommentHandler(IValidator<UpdateCommentRequest> validator, IUnitOfWork unitOfWork, IWorkoutRepository workoutRepository)
        {
            _validator = validator;
            _unitOfWork = unitOfWork;
            _workoutRepository = workoutRepository;
        }

        public async Task<UpdateCommentResponse> Handle(UpdateCommentRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return new UpdateCommentResponse(false, "Validation failure", validationResult.ToDictionary());
            }

            var workout = await _workoutRepository.GetAsync(request.WorkoutId, cancellationToken);

            if (workout == null || workout.Id == 0)
                return new UpdateCommentResponse(false, "Workout is not valid.");

            if (workout.Comments is null || !workout.Comments.Any(c => c.Id == request.CommentId))
                return new UpdateCommentResponse(false, "Comment was not found.");

            var comment = workout.Comments.FirstOrDefault(c => c.Id == request.CommentId);
            comment.UpdateText(request.Text);

            _workoutRepository.Update(workout);
            await _unitOfWork.Save(cancellationToken);

            return new UpdateCommentResponse(true, "Comment successfully update.");
        }
    }

    public record UpdateCommentRequest : IRequest<UpdateCommentResponse>
    {
        [JsonIgnore]
        public int CommentId { get; set; }
        public string Text { get; set; }
        [JsonIgnore]
        public int WorkoutId { get; set; }
    }

    public record UpdateCommentResponse : CommandResult
    {
        public UpdateCommentResponse(bool success, object message, IDictionary<string, string[]>? errors = null) : base(success, message, errors)
        {
        }
    }

    public class UpdateCommentValidator : AbstractValidator<UpdateCommentRequest>
    {
        public UpdateCommentValidator()
        {
            RuleFor(x => x.CommentId).GreaterThan(0);
            RuleFor(x => x.Text).NotEmpty().MinimumLength(3).MaximumLength(300);
            RuleFor(x => x.WorkoutId).GreaterThan(0);
        }
    }

}
