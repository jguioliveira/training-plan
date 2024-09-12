using FluentValidation;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TrainingPlan.API.Application.Features.TeamFeatures.SaveTeamSettings;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;
using Xunit;

public class SaveTeamSettingsHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ITeamRepository> _mockTeamRepository;
    private readonly Mock<IValidator<SaveTeamSettingsRequest>> _mockValidator;
    private readonly SaveTeamSettingsHandler _handler;

    public SaveTeamSettingsHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockTeamRepository = new Mock<ITeamRepository>();
        _mockValidator = new Mock<IValidator<SaveTeamSettingsRequest>>();
        _handler = new SaveTeamSettingsHandler(_mockUnitOfWork.Object, _mockTeamRepository.Object, _mockValidator.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var request = new SaveTeamSettingsRequest { teamId = 1, Settings = new Dictionary<string, string> { { "Key1", "Value1" } } };
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockTeamRepository.Setup(r => r.GetAsync(request.teamId, It.IsAny<CancellationToken>())).ReturnsAsync(new Team("Test Team", "test@example.com"));

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Success);
        Assert.Equal("Team settings were saved.", response.Message);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ReturnsValidationFailureResponse()
    {
        // Arrange
        var request = new SaveTeamSettingsRequest { teamId = 1, Settings = new Dictionary<string, string> { { "Key1", "Value1" } } };
        var validationResult = new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Settings", "Invalid settings") });
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Validation failure", response.Message);
    }

    [Fact]
    public async Task Handle_TeamNotFound_ReturnsFailureResponse()
    {
        // Arrange
        var request = new SaveTeamSettingsRequest { teamId = 1, Settings = new Dictionary<string, string> { { "Key1", "Value1" } } };
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockTeamRepository.Setup(r => r.GetAsync(request.teamId, It.IsAny<CancellationToken>())).ReturnsAsync((Team)null);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Team was not found.", response.Message);
    }
}
