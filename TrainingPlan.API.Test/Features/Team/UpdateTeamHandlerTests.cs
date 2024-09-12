using FluentValidation;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using TrainingPlan.API.Application.Features.TeamFeatures.UpdateTeam;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;
using Xunit;

public class UpdateTeamHandlerTests
{
    private readonly Mock<IValidator<UpdateTeamRequest>> _mockValidator;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ITeamRepository> _mockTeamRepository;
    private readonly UpdateTeamHandler _handler;

    public UpdateTeamHandlerTests()
    {
        _mockValidator = new Mock<IValidator<UpdateTeamRequest>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockTeamRepository = new Mock<ITeamRepository>();
        _handler = new UpdateTeamHandler(_mockUnitOfWork.Object, _mockTeamRepository.Object, _mockValidator.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var request = new UpdateTeamRequest { Id = 1, Name = "Updated Team", Email = "updated@example.com" };
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        var team = new Team("Original Team", "original@example.com") { Id = 1 };
        _mockTeamRepository.Setup(r => r.GetAsync(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(team);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Success);
        Assert.Equal("Team successfully updated.", response.Message);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ReturnsValidationFailureResponse()
    {
        // Arrange
        var request = new UpdateTeamRequest { Id = 1, Name = "Updated Team", Email = "updated@example.com" };
        var validationResult = new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Name", "Invalid name") });
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
        var request = new UpdateTeamRequest { Id = 1, Name = "Updated Team", Email = "updated@example.com" };
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockTeamRepository.Setup(r => r.GetAsync(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync((Team)null);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Team was not found.", response.Message);
    }
}
