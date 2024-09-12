using Moq;
using System.Threading;
using System.Threading.Tasks;
using TrainingPlan.API.Application.Features.PlanFeatures.GetPlan;
using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Repositories;
using Xunit;

public class GetPlanHandlerTests
{
    private readonly Mock<IPlanRepository> _mockPlanRepository;
    private readonly GetPlanHandler _handler;

    public GetPlanHandlerTests()
    {
        _mockPlanRepository = new Mock<IPlanRepository>();
        _handler = new GetPlanHandler(_mockPlanRepository.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsPlanDTO()
    {
        // Arrange
        var request = new GetPlanRequest { AthleteId = 1 };
        var planDto = new PlanDTO { Id = 1, Name = "Test Plan" };
        _mockPlanRepository.Setup(r => r.GetPlanAsync(request.AthleteId)).ReturnsAsync(planDto);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(planDto.Id, result.Id);
        Assert.Equal(planDto.Name, result.Name);
    }

    [Fact]
    public async Task Handle_PlanNotFound_ReturnsNull()
    {
        // Arrange
        var request = new GetPlanRequest { AthleteId = 1 };
        _mockPlanRepository.Setup(r => r.GetPlanAsync(request.AthleteId)).ReturnsAsync((PlanDTO)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
