using FluentValidation;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using TrainingPlan.API.Application.Features.ContentFeatures.UpdateContent;
using TrainingPlan.API.Application.Common.Services;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;
using Xunit;

public class UpdateContentHandlerTests
{
    private readonly Mock<IValidator<UpdateContentRequest>> _mockValidator;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IContentRepository> _mockContentRepository;
    private readonly Mock<IAzureBlobService> _mockAzureBlobService;
    private readonly UpdateContentHandler _handler;

    public UpdateContentHandlerTests()
    {
        _mockValidator = new Mock<IValidator<UpdateContentRequest>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockContentRepository = new Mock<IContentRepository>();
        _mockAzureBlobService = new Mock<IAzureBlobService>();
        _handler = new UpdateContentHandler(_mockValidator.Object, _mockUnitOfWork.Object, _mockContentRepository.Object, _mockAzureBlobService.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var request = new UpdateContentRequest { Id = 1, Title = "Updated Title", Description = "Updated Description", Type = "text/plain", Data = "Updated Data" };
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        var content = new Content { Id = 1, Title = "Original Title", Description = "Original Description", Type = "text/plain", Data = "Original Data" };
        _mockContentRepository.Setup(r => r.GetAsync(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(content);
        _mockAzureBlobService.Setup(s => s.UploadContentToBlobStorage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("Updated Blob Data");

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Success);
        Assert.Equal("Content successfully updated.", response.Message);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ReturnsValidationFailureResponse()
    {
        // Arrange
        var request = new UpdateContentRequest { Id = 1, Title = "Updated Title", Description = "Updated Description", Type = "text/plain", Data = "Updated Data" };
        var validationResult = new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Title", "Invalid title") });
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Validation failure", response.Message);
    }

    [Fact]
    public async Task Handle_ContentNotFound_ReturnsFailureResponse()
    {
        // Arrange
        var request = new UpdateContentRequest { Id = 1, Title = "Updated Title", Description = "Updated Description", Type = "text/plain", Data = "Updated Data" };
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockContentRepository.Setup(r => r.GetAsync(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync((Content)null);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Content not found.", response.Message);
    }
}
