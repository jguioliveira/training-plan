using FluentValidation;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using TrainingPlan.API.Application.Features.ContentFeatures.CreateContent;
using TrainingPlan.API.Application.Common.Services;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;
using Xunit;

public class CreateContentHandlerTests
{
    private readonly Mock<IValidator<CreateContentRequest>> _mockValidator;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IContentRepository> _mockContentRepository;
    private readonly Mock<IAzureBlobService> _mockAzureBlobService;
    private readonly CreateContentHandler _handler;

    public CreateContentHandlerTests()
    {
        _mockValidator = new Mock<IValidator<CreateContentRequest>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockContentRepository = new Mock<IContentRepository>();
        _mockAzureBlobService = new Mock<IAzureBlobService>();
        _handler = new CreateContentHandler(_mockValidator.Object, _mockUnitOfWork.Object, _mockContentRepository.Object, _mockAzureBlobService.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var request = new CreateContentRequest { Title = "New Content", Description = "Description", Type = "text/plain", Data = "Content Data" };
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockAzureBlobService.Setup(s => s.UploadContentToBlobStorage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("BlobUrl");

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Success);
        Assert.Equal("Content successfully created.", response.Message);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ReturnsValidationFailureResponse()
    {
        // Arrange
        var request = new CreateContentRequest { Title = "New Content", Description = "Description", Type = "text/plain", Data = "Content Data" };
        var validationResult = new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Title", "Invalid title") });
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Validation failure", response.Message);
    }
}
