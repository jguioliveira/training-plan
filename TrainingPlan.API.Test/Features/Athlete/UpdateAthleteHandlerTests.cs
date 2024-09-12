using FluentValidation;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using TrainingPlan.API.Application.Features.AthleteFeatures.UpdateAthlete;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;
using Xunit;

public class UpdateAthleteHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IPersonRepository> _mockPersonRepository;
    private readonly Mock<IValidator<UpdateAthleteRequest>> _mockValidator;
    private readonly UpdateAthleteHandler _handler;

    public UpdateAthleteHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockPersonRepository = new Mock<IPersonRepository>();
        _mockValidator = new Mock<IValidator<UpdateAthleteRequest>>();
        _handler = new UpdateAthleteHandler(_mockUnitOfWork.Object, _mockPersonRepository.Object, _mockValidator.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var request = new UpdateAthleteRequest { Id = 1, Name = "Updated Name", Birth = DateTime.UtcNow, Phone = "1234567890" };
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockPersonRepository.Setup(r => r.GetAsync(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(new Athlete(Guid.NewGuid(), "Test Athlete", DateTime.UtcNow, "testathlete@test.com", "1234567890"));

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Success);
        Assert.Equal("Athlete successfully updated.", response.Message);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ReturnsValidationFailureResponse()
    {
        // Arrange
        var request = new UpdateAthleteRequest { Id = 1, Name = "Updated Name", Birth = DateTime.UtcNow, Phone = "1234567890" };
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
        var request = new UpdateAthleteRequest { Id = 1, Name = "Updated Name", Birth = DateTime.UtcNow, Phone = "1234567890" };
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockPersonRepository.Setup(r => r.GetAsync(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync((Athlete)null);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Athlete was not found.", response.Message);
    }
}
