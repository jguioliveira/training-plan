using FluentValidation;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TrainingPlan.API.Application.Features.WorkoutFeatures.UpdateWorkout;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;
using Xunit;

public class UpdateWorkoutHandlerTests
{
    private readonly Mock<IValidator<UpdateWorkoutRequest>> _mockValidator;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IContentRepository> _mockContentRepository;
    private readonly Mock<IWorkoutRepository> _mockWorkoutRepository;
    private readonly UpdateWorkout _handler;

    public UpdateWorkoutHandlerTests()
    {
        _mockValidator = new Mock<IValidator<UpdateWorkoutRequest>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockContentRepository = new Mock<IContentRepository>();
        _mockWorkoutRepository = new Mock<IWorkoutRepository>();
        _handler = new UpdateWorkout(_mockValidator.Object, _mockUnitOfWork.Object, _mockContentRepository.Object, _mockWorkoutRepository.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var request = new UpdateWorkoutRequest { Id = 1, Date = DateTime.UtcNow.AddDays(1), Description = "Updated Description", ContentId = 1 };
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        var workout = new Workout { Id = 1, Date = DateTime.UtcNow, Description = "Original Description", ContentId = 1 };
        _mockWorkoutRepository.Setup(r => r.GetAsync(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(workout);
        var content = new Content { Id = 1, Title = "Content Title", Description = "Content Description", Type = "text/plain", Data = "Content Data" };
        _mockContentRepository.Setup(r => r.GetAsync(request.ContentId.Value, It.IsAny<CancellationToken>())).ReturnsAsync(content);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Success);
        Assert.Equal("Workout successfully saved.", response.Message);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ReturnsValidationFailureResponse()
    {
        // Arrange
        var request = new UpdateWorkoutRequest { Id = 1, Date = DateTime.UtcNow.AddDays(1), Description = "Updated Description", ContentId = 1 };
        var validationResult = new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Description", "Invalid description") });
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Validation failure", response.Message);
    }

    [Fact]
    public async Task Handle_WorkoutNotFound_ReturnsFailureResponse()
    {
        // Arrange
        var request = new UpdateWorkoutRequest { Id = 1, Date = DateTime.UtcNow.AddDays(1), Description = "Updated Description", ContentId = 1 };
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockWorkoutRepository.Setup(r => r.GetAsync(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync((Workout)null);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Workout was not found.", response.Message);
    }

    [Fact]
    public async Task Handle_ContentNotFound_ReturnsFailureResponse()
    {
        // Arrange
        var request = new UpdateWorkoutRequest { Id = 1, Date = DateTime.UtcNow.AddDays(1), Description = "Updated Description", ContentId = 1 };
        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        var workout = new Workout { Id = 1, Date = DateTime.UtcNow, Description = "Original Description", ContentId = 1 };
        _mockWorkoutRepository.Setup(r => r.GetAsync(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(workout);
        _mockContentRepository.Setup(r => r.GetAsync(request.ContentId.Value, It.IsAny<CancellationToken>())).ReturnsAsync((Content)null);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Content was not found.", response.Message);
    }
}
