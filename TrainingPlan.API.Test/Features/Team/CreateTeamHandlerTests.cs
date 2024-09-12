using FluentValidation;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using TrainingPlan.API.Application.Features.TeamFeatures.CreateTeam;
using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;


public class CreateTeamHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ITeamRepository> _mockTeamRepository;
    private readonly Mock<IValidator<CreateTeamRequest>> _mockValidator;
    private readonly CreateTeamHandler _handler;

    public CreateTeamHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockTeamRepository = new Mock<ITeamRepository>();
        _mockValidator = new Mock<IValidator<CreateTeamRequest>>();
        _handler = new CreateTeamHandler(_mockUnitOfWork.Object, _mockTeamRepository.Object, _mockValidator.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var request = new CreateTeamRequest { Name = "Test Team", Email = "test@example.com" };
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockTeamRepository.Setup(r => r.GetTeamAsync(request.Email)).ReturnsAsync(default(TeamDTO));
        _mockTeamRepository.Setup(r => r.Create(It.IsAny<Team>()));
        _mockUnitOfWork.Setup(u => u.Save(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Success);
        Assert.Equal("Team successfully created.", response.Message);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ReturnsValidationFailureResponse()
    {
        // Arrange
        var request = new CreateTeamRequest { Name = "Test Team", Email = "test@example.com" };
        var validationResult = new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Email", "Invalid email") });
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Validation failure", response.Message);
    }

    [Fact]
    public async Task Handle_EmailAlreadyRegistered_ReturnsFailureResponse()
    {
        // Arrange
        var request = new CreateTeamRequest { Name = "Test Team", Email = "test@example.com" };
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockTeamRepository.Setup(r => r.GetTeamAsync(request.Email)).ReturnsAsync(new TeamDTO { Name = "Test Team", Email = "test@example.com" });

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("E-mail already registered.", response.Message);
    }
}
