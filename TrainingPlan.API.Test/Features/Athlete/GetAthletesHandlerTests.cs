using Moq;
using System.Threading;
using System.Threading.Tasks;
using TrainingPlan.API.Application.Features.AthleteFeatures.GetAthletes;
using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Repositories;
using Xunit;

public class GetAthletesHandlerTests
{
    private readonly Mock<IPersonRepository> _mockPersonRepository;
    private readonly GetAthletesHandler _handler;

    public GetAthletesHandlerTests()
    {
        _mockPersonRepository = new Mock<IPersonRepository>();
        _handler = new GetAthletesHandler(_mockPersonRepository.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsAthletesPagedListDTO()
    {
        // Arrange
        var request = new GetAthletesRequest { LastId = 1, PageSize = 10, Direction = "asc", Name = "Test" };
        var athletesPagedListDto = new AthletesPagedListDTO
        {
            Items = new List<AthleteDTO> { new AthleteDTO { Id = 1, Name = "Test Athlete" } },
            Total = 1
        };
        _mockPersonRepository.Setup(r => r.GetAthletesAsync(request.LastId, request.PageSize, request.Direction, request.Name))
                             .ReturnsAsync(athletesPagedListDto);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(athletesPagedListDto.Total, result.Total);
        Assert.Single(result.Items);
        Assert.Equal(athletesPagedListDto.Items.First().Id, result.Items.First().Id);
        Assert.Equal(athletesPagedListDto.Items.First().Name, result.Items.First().Name);
    }

    [Fact]
    public async Task Handle_NoAthletesFound_ReturnsEmptyAthletesPagedListDTO()
    {
        // Arrange
        var request = new GetAthletesRequest { LastId = 1, PageSize = 10, Direction = "asc", Name = "Test" };
        var emptyAthletesPagedListDto = new AthletesPagedListDTO
        {
            Items = new List<AthleteDTO>(),
            Total = 0
        };
        _mockPersonRepository.Setup(r => r.GetAthletesAsync(request.LastId, request.PageSize, request.Direction, request.Name))
                             .ReturnsAsync(emptyAthletesPagedListDto);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Items);
        Assert.Equal(0, result.Total);
    }
}
