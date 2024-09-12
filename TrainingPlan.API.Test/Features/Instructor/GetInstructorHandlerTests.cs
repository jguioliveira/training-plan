using Moq;
using System.Threading;
using System.Threading.Tasks;
using TrainingPlan.API.Application.Features.InstructorFeatures.GetInstructor;
using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Repositories;
using Xunit;

public class GetInstructorHandlerTests
{
    private readonly Mock<IPersonRepository> _mockInstructorRepository;
    private readonly GetInstructorHandler _handler;

    public GetInstructorHandlerTests()
    {
        _mockInstructorRepository = new Mock<IPersonRepository>();
        _handler = new GetInstructorHandler(_mockInstructorRepository.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsInstructorDTO()
    {
        // Arrange
        var request = new GetInstructorRequest { Id = 1 };
        var instructorDto = new InstructorDTO { Id = 1, Name = "Test Instructor" };
        _mockInstructorRepository.Setup(r => r.GetInstructorAsync(request.Id)).ReturnsAsync(instructorDto);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(instructorDto.Id, result.Id);
        Assert.Equal(instructorDto.Name, result.Name);
    }

    [Fact]
    public async Task Handle_InstructorNotFound_ReturnsNull()
    {
        // Arrange
        var request = new GetInstructorRequest { Id = 1 };
        _mockInstructorRepository.Setup(r => r.GetInstructorAsync(request.Id)).ReturnsAsync((InstructorDTO?)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
