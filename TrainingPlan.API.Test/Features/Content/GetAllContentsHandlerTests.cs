using Moq;
using System.Threading;
using System.Threading.Tasks;
using TrainingPlan.API.Application.Features.ContentFeatures.GetAllContents;
using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Repositories;
using Xunit;

public class GetAllContentsHandlerTests
{
    private readonly Mock<IContentRepository> _mockContentRepository;
    private readonly GetAllContentsHandler _handler;

    public GetAllContentsHandlerTests()
    {
        _mockContentRepository = new Mock<IContentRepository>();
        _handler = new GetAllContentsHandler(_mockContentRepository.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsContentsPagedListDTO()
    {
        // Arrange
        var request = new GetAllContentsRequest { LastId = 1, PageSize = 10, Direction = "asc", Title = "Test" };
        var contentsPagedListDto = new ContentsPagedListDTO
        {
            Items = new List<ContentDTO> { new ContentDTO { Id = 1, Title = "Test Content" } },
            Total = 1
        };
        _mockContentRepository.Setup(r => r.GetContentAsync(request.LastId, request.PageSize, request.Direction, request.Title))
                              .ReturnsAsync(contentsPagedListDto);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(contentsPagedListDto.Total, result.Total);
        Assert.Single(result.Items);
        Assert.Equal(contentsPagedListDto.Items.First().Id, result.Items.First().Id);
        Assert.Equal(contentsPagedListDto.Items.First().Title, result.Items.First().Title);
    }

    [Fact]
    public async Task Handle_NoContentsFound_ReturnsEmptyContentsPagedListDTO()
    {
        // Arrange
        var request = new GetAllContentsRequest { LastId = 1, PageSize = 10, Direction = "asc", Title = "Test" };
        var emptyContentsPagedListDto = new ContentsPagedListDTO
        {
            Items = new List<ContentDTO>(),
            Total = 0
        };
        _mockContentRepository.Setup(r => r.GetContentAsync(request.LastId, request.PageSize, request.Direction, request.Title))
                              .ReturnsAsync(emptyContentsPagedListDto);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Items);
        Assert.Equal(0, result.Total);
    }
}
