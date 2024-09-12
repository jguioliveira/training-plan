using Moq;
using System.Threading;
using System.Threading.Tasks;
using TrainingPlan.API.Application.Features.TeamFeatures.GetTeam;
using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Repositories;
using Xunit;

public class GetTeamHandlerTests
{
    private readonly Mock<ITeamRepository> _mockTeamRepository;
    private readonly GetTeamHandler _handler;

    public GetTeamHandlerTests()
    {
        _mockTeamRepository = new Mock<ITeamRepository>();
        _handler = new GetTeamHandler(_mockTeamRepository.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsTeamDTO()
    {
        // Arrange
        var request = new GetTeamRequest { Id = 1 };
        var teamDto = new TeamDTO { Id = 1, Name = "Test Team" };
        _mockTeamRepository.Setup(r => r.GetTeamAsync(request.Id)).ReturnsAsync(teamDto);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(teamDto.Id, result.Id);
        Assert.Equal(teamDto.Name, result.Name);
    }

    [Fact]
    public async Task Handle_TeamNotFound_ReturnsNull()
    {
        // Arrange
        var request = new GetTeamRequest { Id = 1 };
        _mockTeamRepository.Setup(r => r.GetTeamAsync(request.Id)).ReturnsAsync((TeamDTO)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
