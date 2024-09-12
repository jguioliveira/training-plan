using FluentValidation;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using TrainingPlan.API.Application.Features.InstructorFeatures.UpdateInstructor;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;
using Xunit;

public class UpdateInstructorHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IPersonRepository> _mockInstructorRepository;
    private readonly Mock<IValidator<UpdateInstructorRequest>> _mockValidator;
    private readonly UpdateInstructorHandler _handler;

    public UpdateInstructorHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockInstructorRepository = new Mock<IPersonRepository>();
        _mockValidator = new Mock<IValidator<UpdateInstructorRequest>>();
        _handler = new UpdateInstructorHandler(_mockUnitOfWork.Object, _mockInstructorRepository.Object, _mockValidator.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var request = new UpdateInstructorRequest { Id = 1, Name = "Updated Name", Birth = DateTime.Now.AddYears(-30), Phone = "1234567890" };
        var instructor = new Instructor(Guid.NewGuid(), "Original Name", DateTime.Now.AddYears(-40), "originalName@teste.com" , "0987654321" );

        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockInstructorRepository.Setup(r => r.GetAsync(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync(instructor);
        _mockUnitOfWork.Setup(u => u.Save(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Success);
        Assert.Equal("Instructor successfully updated.", response.Message);
        _mockInstructorRepository.Verify(r => r.Update(It.IsAny<Instructor>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.Save(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidRequest_ReturnsValidationFailureResponse()
    {
        // Arrange
        var request = new UpdateInstructorRequest { Id = 1, Name = "Updated Name", Birth = DateTime.Now.AddYears(-30), Phone = "1234567890" };
        var validationResult = new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Name", "Invalid Name") });

        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Validation failure", response.Message);
        _mockInstructorRepository.Verify(r => r.Update(It.IsAny<Instructor>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.Save(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_InstructorNotFound_ReturnsNotFoundResponse()
    {
        // Arrange
        var request = new UpdateInstructorRequest { Id = 1, Name = "Updated Name", Birth = DateTime.Now.AddYears(-30), Phone = "1234567890" };

        _mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
        _mockInstructorRepository.Setup(r => r.GetAsync(request.Id, It.IsAny<CancellationToken>())).ReturnsAsync((Instructor)null);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Instructor was not found.", response.Message);
        _mockInstructorRepository.Verify(r => r.Update(It.IsAny<Instructor>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.Save(It.IsAny<CancellationToken>()), Times.Never);
    }
}
