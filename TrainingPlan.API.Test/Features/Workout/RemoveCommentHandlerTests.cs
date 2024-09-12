using FluentValidation;
using Moq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrainingPlan.API.Application.Features.WorkoutFeatures.RemoveComment;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;
using Xunit;

public class RemoveCommentHandlerTests
{
    private readonly Mock<IValidator<RemoveCommentRequest>> _mockValidator;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IWorkoutRepository> _mockWorkoutRepository;
    private readonly RemoveCommentHandler _handler;

    public RemoveCommentHandlerTests()
    {
        _mockValidator = new Mock<IValidator<RemoveCommentRequest>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockWorkoutRepository = new Mock<IWorkoutRepository>();
        _handler = new RemoveCommentHandler(_mockValidator.Object, _mockUnitOfWork.Object, _mockWorkoutRepository.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var request = new RemoveCommentRequest { WorkoutId = 1, CommentId = 1 };
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        var workout = new Workout { Id = 1, Comments = new List<Comment> { new Comment { Id = 1, Text = "Test Comment" } } };
        _mockWorkoutRepository.Setup(r => r.GetAsync(request.WorkoutId, It.IsAny<CancellationToken>())).ReturnsAsync(workout);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Success);
        Assert.Equal("Comment successfully update.", response.Message);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ReturnsValidationFailureResponse()
    {
        // Arrange
        var request = new RemoveCommentRequest { WorkoutId = 1, CommentId = 1 };
        var validationResult = new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("WorkoutId", "Invalid WorkoutId") });
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Validation failure", response.Message);
    }

    [Fact]
    public async Task Handle_WorkoutNotFound_ReturnsFailureResponse()
    {
        // Arrange
        var request = new RemoveCommentRequest { WorkoutId = 1, CommentId = 1 };
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockWorkoutRepository.Setup(r => r.GetAsync(request.WorkoutId, It.IsAny<CancellationToken>())).ReturnsAsync((Workout)null);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Workout is not valid.", response.Message);
    }

    [Fact]
    public async Task Handle_CommentNotFound_ReturnsFailureResponse()
    {
        // Arrange
        var request = new RemoveCommentRequest { WorkoutId = 1, CommentId = 1 };
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        var workout = new Workout { Id = 1, Comments = new List<Comment> { new Comment { Id = 2, Text = "Another Comment" } } };
        _mockWorkoutRepository.Setup(r => r.GetAsync(request.WorkoutId, It.IsAny<CancellationToken>())).ReturnsAsync(workout);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Comment was not found.", response.Message);
    }
}
