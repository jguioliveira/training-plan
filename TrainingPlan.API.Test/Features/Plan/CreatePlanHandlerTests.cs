using FluentValidation;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TrainingPlan.API.Application.Features.PlanFeatures.CreatePlan;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;
using Xunit;

public class CreatePlanHandlerTests
{
    private readonly Mock<IValidator<CreatePlanRequest>> _mockValidator;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IPlanRepository> _mockPlanRepository;
    private readonly Mock<IPersonRepository> _mockPersonRepository;
    private readonly CreatePlanHandler _handler;

    public CreatePlanHandlerTests()
    {
        _mockValidator = new Mock<IValidator<CreatePlanRequest>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockPlanRepository = new Mock<IPlanRepository>();
        _mockPersonRepository = new Mock<IPersonRepository>();
        _handler = new CreatePlanHandler(_mockValidator.Object, _mockUnitOfWork.Object, _mockPlanRepository.Object, _mockPersonRepository.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var request = new CreatePlanRequest { Name = "New Plan", Goal = "Goal", AhtleteId = 1, InstructorId = 1, Description = "Description" };
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        var athlete = new Person { Id = 1, Name = "Athlete" };
        var instructor = new Person { Id = 1, Name = "Instructor" };
        _mockPersonRepository.Setup(r => r.GetAsync(request.AhtleteId, It.IsAny<CancellationToken>())).ReturnsAsync(athlete);
        _mockPersonRepository.Setup(r => r.GetAsync(request.InstructorId, It.IsAny<CancellationToken>())).ReturnsAsync(instructor);

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
        var request = new CreatePlanRequest { Name = "New Plan", Goal = "Goal", AhtleteId = 1, InstructorId = 1, Description = "Description" };
        var validationResult = new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Name", "Invalid name") });
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Validation failure", response.Message);
    }

    [Fact]
    public async Task Handle_AthleteNotFound_ReturnsFailureResponse()
    {
        // Arrange
        var request = new CreatePlanRequest { Name = "New Plan", Goal = "Goal", AhtleteId = 1, InstructorId = 1, Description = "Description" };
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockPersonRepository.Setup(r => r.GetAsync(request.AhtleteId, It.IsAny<CancellationToken>())).ReturnsAsync((Person)null);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Athlete is not valid.", response.Message);
    }

    [Fact]
    public async Task Handle_InstructorNotFound_ReturnsFailureResponse()
    {
        // Arrange
        var request = new CreatePlanRequest { Name = "New Plan", Goal = "Goal", AhtleteId = 1, InstructorId = 1, Description = "Description" };
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        var athlete = new Person { Id = 1, Name = "Athlete" };
        _mockPersonRepository.Setup(r => r.GetAsync(request.AhtleteId, It.IsAny<CancellationToken>())).ReturnsAsync(athlete);
        _mockPersonRepository.Setup(r => r.GetAsync(request.InstructorId, It.IsAny<CancellationToken>())).ReturnsAsync((Person)null);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Instructor is not valid.", response.Message);
    }
}
