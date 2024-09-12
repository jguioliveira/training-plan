using FluentValidation;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using TrainingPlan.API.Application.Features.PlanFeatures.UpdatePlan;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;
using Xunit;

public class UpdatePlanHandlerTests
{
    private readonly Mock<IValidator<UpdatePlanRequest>> _mockValidator;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IPlanRepository> _mockPlanRepository;
    private readonly Mock<IPersonRepository> _mockPersonRepository;
    private readonly UpdatePlanHandler _handler;

    public UpdatePlanHandlerTests()
    {
        _mockValidator = new Mock<IValidator<UpdatePlanRequest>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockPlanRepository = new Mock<IPlanRepository>();
        _mockPersonRepository = new Mock<IPersonRepository>();
        _handler = new UpdatePlanHandler(_mockValidator.Object, _mockUnitOfWork.Object, _mockPlanRepository.Object, _mockPersonRepository.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var request = new UpdatePlanRequest { Id = 1, Name = "Updated Plan", Description = "Updated Description", Goal = "Updated Goal", InstructorId = 1 };
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockPlanRepository.Setup(r => r.GetAsync(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(new Plan("Test Plan", "Test Description", "Test Goal", 1));
        _mockPersonRepository.Setup(r => r.GetAsync(request.InstructorId, It.IsAny<CancellationToken>())).ReturnsAsync(new Instructor("Test Instructor"));

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Success);
        Assert.Equal("Plan successfully created.", response.Message);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ReturnsValidationFailureResponse()
    {
        // Arrange
        var request = new UpdatePlanRequest { Id = 1, Name = "Updated Plan", Description = "Updated Description", Goal = "Updated Goal", InstructorId = 1 };
        var validationResult = new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Name", "Invalid name") });
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
        var request = new UpdatePlanRequest { Id = 1, Name = "Updated Plan", Description = "Updated Description", Goal = "Updated Goal", InstructorId = 1 };
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockPlanRepository.Setup(r => r.GetAsync(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync((Plan)null);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Plan was not found.", response.Message);
    }

    [Fact]
    public async Task Handle_InvalidInstructor_ReturnsFailureResponse()
    {
        // Arrange
        var request = new UpdatePlanRequest { Id = 1, Name = "Updated Plan", Description = "Updated Description", Goal = "Updated Goal", InstructorId = 1 };
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockPlanRepository.Setup(r => r.GetAsync(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(new Plan("Test Plan", "Test Description", "Test Goal", 1));
        _mockPersonRepository.Setup(r => r.GetAsync(request.InstructorId, It.IsAny<CancellationToken>())).ReturnsAsync((Instructor)null);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Instructor is not valid.", response.Message);
    }
}
