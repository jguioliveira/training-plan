using FluentValidation;
using MediatR;
using TrainingPlan.API.Application.Common.Commands;
using TrainingPlan.API.Application.Common.Services;
using TrainingPlan.Domain.Repositories;

namespace TrainingPlan.API.Application.Features.ContentFeatures.UpdateContent
{
    public class UpdateContentHandler : IRequestHandler<UpdateContentRequest, UpdateContentResponse>
    {
        private readonly IValidator<UpdateContentRequest> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IContentRepository _contentRepository;
        private readonly IAzureBlobService _azureBlobService;

        public UpdateContentHandler(IValidator<UpdateContentRequest> validator, IUnitOfWork unitOfWork, IContentRepository contentRepository, IAzureBlobService azureBlobService)
        {
            _validator = validator;
            _unitOfWork = unitOfWork;
            _contentRepository = contentRepository;
            _azureBlobService = azureBlobService;
        }

        public async Task<UpdateContentResponse> Handle(UpdateContentRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return new UpdateContentResponse(false, "Validation failure", validationResult.ToDictionary());
            }

            var content = await _contentRepository.GetAsync(request.Id, cancellationToken);

            if(content == null || content.Id == 0)
            {
                return new UpdateContentResponse(false, "Content not found.");
            }

            if (!string.IsNullOrEmpty(request.Title))
            {
                content.UpdateTitle(request.Title);
            }

            if (!string.IsNullOrEmpty(request.Description))
            {
                content.Description = request.Description;
            }

            if (!string.IsNullOrEmpty(request.Data) && !string.IsNullOrEmpty(request.Type))
            {
                string blobData = await _azureBlobService.UploadContentToBlobStorage(content.Title, content.Type, request.Data);

                content.UpdateData(blobData, request.Type);
            }

            _contentRepository.Update(content);
            await _unitOfWork.Save(cancellationToken);

            return new UpdateContentResponse(true, "Content successfully updated.");
        }

    }

    public record UpdateContentRequest : IRequest<UpdateContentResponse>
    {
        public int Id { get; set; }
        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Type { get; set; }

        public string? Data { get; set; }
    }

    public record UpdateContentResponse : CommandResult
    {
        public UpdateContentResponse(bool success, object message, IDictionary<string, string[]>? errors = null) : base(success, message, errors)
        {
        }
    }

    public class UpdateContentValidator : AbstractValidator<UpdateContentRequest>
    {
        public UpdateContentValidator()
        {
            RuleFor(x => x.Title).MinimumLength(3).MaximumLength(50);
            RuleFor(x => x.Type).MinimumLength(3);
            RuleFor(x => x.Description).MaximumLength(200);
        }
    }

}
