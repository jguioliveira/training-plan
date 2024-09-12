using FluentValidation;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using TrainingPlan.API.Application.Features.AthleteFeatures.CreateAthlete;
using TrainingPlan.API.Application.Common.Services;
using Xunit;

public class CreateAthleteHandlerTests
{
    private readonly Mock<IValidator<CreateAthleteRequest>> _mockValidator;
    private readonly Mock<IUserService> _mockUserService;
    private readonly CreateAthleteHandler _handler;

    public CreateAthleteHandlerTests()
    {
        _mockValidator = new Mock<IValidator<CreateAthleteRequest>>();
        _mockUserService = new Mock<IUserService>();
        _handler = new CreateAthleteHandler(_mockValidator.Object, _mockUserService.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var request = new CreateAthleteRequest("test@example.com", "password123", "Test User", DateTime.UtcNow, "1234567890");
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockUserService.Setup(s => s.CreateAthleteAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Success);
        Assert.Equal("Athlete successfully created.", response.Message);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ReturnsValidationFailureResponse()
    {
        // Arrange
        var request = new CreateAthleteRequest("test@example.com", "password123", "Test User", DateTime.UtcNow, "1234567890");
        var validationResult = new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Email", "Invalid email") });
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Validation failure", response.Message);
    }
}
