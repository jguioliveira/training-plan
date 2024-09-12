using Moq;
using System.Threading;
using System.Threading.Tasks;
using TrainingPlan.API.Application.Features.AthleteFeatures.GetAthlete;
using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Repositories;
using Xunit;

public class GetAthleteHandlerTests
{
    private readonly Mock<IPersonRepository> _mockPersonRepository;
    private readonly GetAthleteHandler _handler;

    public GetAthleteHandlerTests()
    {
        _mockPersonRepository = new Mock<IPersonRepository>();
        _handler = new GetAthleteHandler(_mockPersonRepository.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsAthleteDTO()
    {
        // Arrange
        var request = new GetAthleteRequest { Id = 1 };
        var athleteDto = new AthleteDTO { Id = 1, Name = "Test Athlete" };
        _mockPersonRepository.Setup(r => r.GetAthleteAsync(request.Id)).ReturnsAsync(athleteDto);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(athleteDto.Id, result.Id);
        Assert.Equal(athleteDto.Name, result.Name);
    }

    [Fact]
    public async Task Handle_AthleteNotFound_ReturnsNull()
    {
        // Arrange
        var request = new GetAthleteRequest { Id = 1 };
        _mockPersonRepository.Setup(r => r.GetAthleteAsync(request.Id)).ReturnsAsync((AthleteDTO)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
