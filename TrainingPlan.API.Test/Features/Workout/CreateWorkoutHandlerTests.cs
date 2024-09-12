using FluentValidation;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using TrainingPlan.API.Application.Features.WorkoutFeatures.CreateWorkout;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;
using Xunit;

public class CreateWorkoutHandlerTests
{
    private readonly Mock<IValidator<CreateWorkoutRequest>> _mockValidator;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IPlanRepository> _mockPlanRepository;
    private readonly Mock<IWorkoutRepository> _mockWorkoutRepository;
    private readonly CreateWorkoutHandler _handler;

    public CreateWorkoutHandlerTests()
    {
        _mockValidator = new Mock<IValidator<CreateWorkoutRequest>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockPlanRepository = new Mock<IPlanRepository>();
        _mockWorkoutRepository = new Mock<IWorkoutRepository>();
        _handler = new CreateWorkoutHandler(_mockValidator.Object, _mockUnitOfWork.Object, _mockPlanRepository.Object, _mockWorkoutRepository.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var request = new CreateWorkoutRequest { PlanId = 1, Date = DateTime.UtcNow, Description = "Test Workout", ContentId = 1 };
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockPlanRepository.Setup(r => r.GetAsync(request.PlanId, It.IsAny<CancellationToken>())).ReturnsAsync(new Plan("Plan 1", "Maratona", 1, 1));
        _mockWorkoutRepository.Setup(r => r.Create(It.IsAny<Workout>()));
        _mockUnitOfWork.Setup(u => u.Save(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Success);
        Assert.Equal("Workout successfully saved.", response.Message);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ReturnsValidationFailureResponse()
    {
        // Arrange
        var request = new CreateWorkoutRequest { PlanId = 1, Date = DateTime.UtcNow, Description = "Test Workout", ContentId = 1 };
        var validationResult = new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("PlanId", "Invalid PlanId") });
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Validation failure", response.Message);
    }

    [Fact]
    public async Task Handle_PlanNotFound_ReturnsFailureResponse()
    {
        // Arrange
        var request = new CreateWorkoutRequest { PlanId = 1, Date = DateTime.UtcNow, Description = "Test Workout", ContentId = 1 };
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockPlanRepository.Setup(r => r.GetAsync(request.PlanId, It.IsAny<CancellationToken>())).ReturnsAsync((Plan)null);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Plan was not found.", response.Message);
    }
}
