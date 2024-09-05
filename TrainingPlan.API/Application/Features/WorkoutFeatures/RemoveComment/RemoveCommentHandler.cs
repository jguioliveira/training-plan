using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrainingPlan.API.Application.Common.Commands;
using TrainingPlan.Domain.Repositories;

namespace TrainingPlan.API.Application.Features.WorkoutFeatures.RemoveComment
{
    public class RemoveCommentHandler : IRequestHandler<RemoveCommentRequest, RemoveCommentResponse>
    {
        private readonly IValidator<RemoveCommentRequest> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWorkoutRepository _workoutRepository;

        public RemoveCommentHandler(IValidator<RemoveCommentRequest> validator, IUnitOfWork unitOfWork, IWorkoutRepository workoutRepository)
        {
            _validator = validator;
            _unitOfWork = unitOfWork;
            _workoutRepository = workoutRepository;
        }

        public async Task<RemoveCommentResponse> Handle(RemoveCommentRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return new RemoveCommentResponse(false, "Validation failure", validationResult.ToDictionary());
            }

            var workout = await _workoutRepository.GetAsync(request.WorkoutId, cancellationToken);

            if (workout == null || workout.Id == 0)
                return new RemoveCommentResponse(false, "Workout is not valid.");

            if (workout.Comments is null || !workout.Comments.Any(c => c.Id == request.CommentId))
                return new RemoveCommentResponse(false, "Comment was not found.");

            var comment = workout.Comments.FirstOrDefault(c => c.Id == request.CommentId);
            comment.Remove();

            _workoutRepository.Update(workout);
            await _unitOfWork.Save(cancellationToken);

            return new RemoveCommentResponse(true, "Comment successfully update.");
        }
    }

    public record RemoveCommentRequest : IRequest<RemoveCommentResponse>
    {
        [FromRoute]
        public int WorkoutId { get; set; }
        [FromRoute]
        public int CommentId { get; set; }
    }

    public record RemoveCommentResponse : CommandResult
    {
        public RemoveCommentResponse(bool success, object message, IDictionary<string, string[]>? errors = null) : base(success, message, errors)
        {
        }
    }

    public class RemoveCommentValidator : AbstractValidator<RemoveCommentRequest>
    {
        public RemoveCommentValidator()
        {
            RuleFor(x => x.WorkoutId).GreaterThan(0);
            RuleFor(x => x.CommentId).GreaterThan(0);
        }
    }

}
