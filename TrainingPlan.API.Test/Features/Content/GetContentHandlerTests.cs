using Moq;
using System.Threading;
using System.Threading.Tasks;
using TrainingPlan.API.Application.Features.ContentFeatures.GetContent;
using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Repositories;
using Xunit;

public class GetContentHandlerTests
{
    private readonly Mock<IContentRepository> _mockContentRepository;
    private readonly GetContentHandler _handler;

    public GetContentHandlerTests()
    {
        _mockContentRepository = new Mock<IContentRepository>();
        _handler = new GetContentHandler(_mockContentRepository.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsContentDTO()
    {
        // Arrange
        var request = new GetContentRequest { Id = 1 };
        var contentDto = new ContentDTO { Id = 1, Title = "Test Content" };
        _mockContentRepository.Setup(r => r.GetContentAsync(request.Id)).ReturnsAsync(contentDto);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(contentDto.Id, result.Id);
        Assert.Equal(contentDto.Title, result.Title);
    }

    [Fact]
    public async Task Handle_ContentNotFound_ReturnsNull()
    {
        // Arrange
        var request = new GetContentRequest { Id = 1 };
        _mockContentRepository.Setup(r => r.GetContentAsync(request.Id)).ReturnsAsync((ContentDTO)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
