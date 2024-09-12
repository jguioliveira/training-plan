using FluentValidation;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using TrainingPlan.API.Application.Features.TeamFeatures.SaveTeamSocialMedia;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;
using Xunit;

public class SaveTeamSocialMediaHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ITeamRepository> _mockTeamRepository;
    private readonly Mock<IValidator<SaveTeamSocialMediaRequest>> _mockValidator;
    private readonly SaveTeamSocialMediaHandler _handler;

    public SaveTeamSocialMediaHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockTeamRepository = new Mock<ITeamRepository>();
        _mockValidator = new Mock<IValidator<SaveTeamSocialMediaRequest>>();
        _handler = new SaveTeamSocialMediaHandler(_mockUnitOfWork.Object, _mockTeamRepository.Object, _mockValidator.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var request = new SaveTeamSocialMediaRequest
        {
            Id = 1,
            SocialMedia = new SaveTeamSocialMediaRequest.SaveSocialMediaRequest
            {
                Name = "Facebook",
                Account = "testAccount"
            }
        };
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockTeamRepository.Setup(r => r.GetAsync(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(new Team("Test Team", "test@example.com"));

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Success);
        Assert.Equal("Team social media was saved.", response.Message);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ReturnsValidationFailureResponse()
    {
        // Arrange
        var request = new SaveTeamSocialMediaRequest
        {
            Id = 1,
            SocialMedia = new SaveTeamSocialMediaRequest.SaveSocialMediaRequest
            {
                Name = "Facebook",
                Account = "testAccount"
            }
        };
        var validationResult = new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("SocialMedia", "Invalid social media") });
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
        var request = new SaveTeamSocialMediaRequest
        {
            Id = 1,
            SocialMedia = new SaveTeamSocialMediaRequest.SaveSocialMediaRequest
            {
                Name = "Facebook",
                Account = "testAccount"
            }
        };
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockTeamRepository.Setup(r => r.GetAsync(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync((Team)null);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Team was not found.", response.Message);
    }
}
