using Moq;
using System.Threading;
using System.Threading.Tasks;
using TrainingPlan.API.Application.Features.InstructorFeatures.GetAllInstructors;
using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Repositories;
using Xunit;

public class GetAllInstructorsHandlerTests
{
    private readonly Mock<IPersonRepository> _mockInstructorRepository;
    private readonly GetAllInstructorsHandler _handler;

    public GetAllInstructorsHandlerTests()
    {
        _mockInstructorRepository = new Mock<IPersonRepository>();
        _handler = new GetAllInstructorsHandler(_mockInstructorRepository.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsInstructorsPagedListDTO()
    {
        // Arrange
        var request = new GetAllInstructorsRequest { LastId = 0, PageSize = 10, Direction = "asc", Name = "Test" };
        var instructorsPagedListDto = new InstructorsPagedListDTO
        {
            Items = new List<InstructorDTO> { new InstructorDTO { Id = 1, Name = "Test Instructor" } },
            Total = 1
        };
        _mockInstructorRepository.Setup(r => r.GetInstructorsAsync(request.LastId, request.PageSize, request.Direction, request.Name)).ReturnsAsync(instructorsPagedListDto);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(instructorsPagedListDto.Total, result.Total);
        Assert.Single(result.Items);
        Assert.Equal(instructorsPagedListDto.Items.First().Id, result.Items.First().Id);
    }

    [Fact]
    public async Task Handle_NoInstructorsFound_ReturnsEmptyInstructorsPagedListDTO()
    {
        // Arrange
        var request = new GetAllInstructorsRequest { LastId = 0, PageSize = 10, Direction = "asc", Name = "Test" };
        var instructorsPagedListDto = new InstructorsPagedListDTO
        {
            Items = new List<InstructorDTO>(),
            Total = 0
        };
        _mockInstructorRepository.Setup(r => r.GetInstructorsAsync(request.LastId, request.PageSize, request.Direction, request.Name)).ReturnsAsync(instructorsPagedListDto);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Items);
        Assert.Equal(0, result.Total);
    }
}

