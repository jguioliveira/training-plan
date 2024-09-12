using FluentValidation;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using TrainingPlan.API.Application.Features.WorkoutFeatures.CreateComment;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;
using Xunit;

public class CreateCommentHandlerTests
{
    private readonly Mock<IValidator<CreateWorkoutCommentRequest>> _mockValidator;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IWorkoutRepository> _mockWorkoutRepository;
    private readonly Mock<IPersonRepository> _mockPersonRepository;
    private readonly CreateCommentHandler _handler;

    public CreateCommentHandlerTests()
    {
        _mockValidator = new Mock<IValidator<CreateWorkoutCommentRequest>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockWorkoutRepository = new Mock<IWorkoutRepository>();
        _mockPersonRepository = new Mock<IPersonRepository>();
        _handler = new CreateCommentHandler(_mockValidator.Object, _mockUnitOfWork.Object, _mockWorkoutRepository.Object, _mockPersonRepository.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var request = new CreateWorkoutCommentRequest { WorkoutId = 1, PersonId = 1, Text = "Great workout!" };
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        var person = new Person { Id = 1, Name = "John Doe", Type = "Athlete" };
        var workout = new Workout { Id = 1 };
        _mockPersonRepository.Setup(r => r.GetAsync(request.PersonId, It.IsAny<CancellationToken>())).ReturnsAsync(person);
        _mockWorkoutRepository.Setup(r => r.GetAsync(request.WorkoutId, It.IsAny<CancellationToken>())).ReturnsAsync(workout);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Success);
        Assert.Equal("Comment successfully created.", response.Message);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ReturnsValidationFailureResponse()
    {
        // Arrange
        var request = new CreateWorkoutCommentRequest { WorkoutId = 1, PersonId = 1, Text = "Great workout!" };
        var validationResult = new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Text", "Invalid text") });
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Validation failure", response.Message);
    }

    [Fact]
    public async Task Handle_PersonNotFound_ReturnsFailureResponse()
    {
        // Arrange
        var request = new CreateWorkoutCommentRequest { WorkoutId = 1, PersonId = 1, Text = "Great workout!" };
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockPersonRepository.Setup(r => r.GetAsync(request.PersonId, It.IsAny<CancellationToken>())).ReturnsAsync((Person)null);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Person is not valid.", response.Message);
    }

    [Fact]
    public async Task Handle_WorkoutNotFound_ReturnsFailureResponse()
    {
        // Arrange
        var request = new CreateWorkoutCommentRequest { WorkoutId = 1, PersonId = 1, Text = "Great workout!" };
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        var person = new Person { Id = 1, Name = "John Doe", Type = "Athlete" };
        _mockPersonRepository.Setup(r => r.GetAsync(request.PersonId, It.IsAny<CancellationToken>())).ReturnsAsync(person);
        _mockWorkoutRepository.Setup(r => r.GetAsync(request.WorkoutId, It.IsAny<CancellationToken>())).ReturnsAsync((Workout)null);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Workout is not valid.", response.Message);
    }
}
