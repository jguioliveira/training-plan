using FluentValidation;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using TrainingPlan.API.Application.Features.InstructorFeatures.CreateInstructor;
using TrainingPlan.API.Application.Common.Services;
using Xunit;

public class CreateInstructorHandlerTests
{
    private readonly Mock<IValidator<CreateInstructorRequest>> _mockValidator;
    private readonly Mock<IUserService> _mockUserService;
    private readonly CreateInstructorHandler _handler;

    public CreateInstructorHandlerTests()
    {
        _mockValidator = new Mock<IValidator<CreateInstructorRequest>>();
        _mockUserService = new Mock<IUserService>();
        _handler = new CreateInstructorHandler(_mockValidator.Object, _mockUserService.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var request = new CreateInstructorRequest("test@example.com", "Password123", "Test Instructor", DateTime.Now.AddYears(-30), "1234567890");
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockUserService.Setup(s => s.CreateInstructorAsync(request.Name, request.Email, request.Password, request.Birth, request.Phone, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Success);
        Assert.Equal("Instructor successfully created.", response.Message);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ReturnsValidationFailureResponse()
    {
        // Arrange
        var request = new CreateInstructorRequest("test@example.com", "Password123", "Test Instructor", DateTime.Now.AddYears(-30), "1234567890");
        var validationResult = new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Name", "Invalid Name") });
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Validation failure", response.Message);
    }
}
