using FluentValidation;
using MediatR;
using TrainingPlan.API.Application.Common.Commands;
using TrainingPlan.API.Application.Common.Services;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;

namespace TrainingPlan.API.Application.Features.ContentFeatures.CreateContent
{
    public class CreateContentHandler : IRequestHandler<CreateContentRequest, CreateContentResponse>
    {
        private readonly IValidator<CreateContentRequest> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IContentRepository _contentRepository;
        private readonly IAzureBlobService _azureBlobService;

        public CreateContentHandler(IValidator<CreateContentRequest> validator, IUnitOfWork unitOfWork, IContentRepository contentRepository, IAzureBlobService azureBlobService)
        {
            _validator = validator;
            _unitOfWork = unitOfWork;
            _contentRepository = contentRepository;
            _azureBlobService = azureBlobService;
        }

        public async Task<CreateContentResponse> Handle(CreateContentRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return new CreateContentResponse(false, "Validation failure", validationResult.ToDictionary());
            }

            //upload the content to the destination (Azure Blob Storage / S3 / etc)
            var blobUrl = await _azureBlobService.UploadContentToBlobStorage(request.Title, request.Type, request.Data);

            var content = new Content(request.Title, blobUrl, request.Type)
            {
                Description = request.Description
            };

            _contentRepository.Create(content);
            await _unitOfWork.Save(cancellationToken);

            return new CreateContentResponse(true, "Content successfully created.");
        }

    }

    public record CreateContentRequest : IRequest<CreateContentResponse>
    {
        public string Title { get; set; }

        public string? Description { get; set; }

        public string Type { get; set; }

        public string Data { get; set; }
    }

    public record CreateContentResponse : CommandResult
    {
        public CreateContentResponse(bool success, object message, IDictionary<string, string[]>? errors = null) : base(success, message, errors)
        {
        }
    }

    public class CreateContentValidator : AbstractValidator<CreateContentRequest>
    {
        public CreateContentValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MinimumLength(3).MaximumLength(50);
            RuleFor(x => x.Type).NotEmpty().MinimumLength(3);
            RuleFor(x => x.Description).MaximumLength(200);
            RuleFor(x => x.Data).NotEmpty();
        }
    }

}

//private const string accessToken = "<Your_Access_Token>";
//private const string filePath = "<Path_To_Your_Video_File>";

//static async Task Main(string[] args)
//{
//    var vimeoClient = new VimeoClient(accessToken);

//    try
//    {
//        // Upload the video file
//        var fileInfo = new FileInfo(filePath);
//        var uploadRequest = await vimeoClient.UploadEntireFileAsync(fileInfo.FullName);

//        // Get the video details
//        var video = await vimeoClient.GetVideoAsync(uploadRequest.ClipUri);

//        Console.WriteLine($"Video uploaded successfully: {video.link}");
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Error uploading video: {ex.Message}");
//    }
//}


//private const string connectionString = "<Your_Connection_String>";
//private const string containerName = "<Your_Container_Name>";
//private const string blobName = "<Your_Blob_Name>";
//private const string filePath = "<Path_To_Your_File>";

//static async Task Main(string[] args)
//{
//    // Create a BlobServiceClient object which will be used to create a container client
//    BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

//    // Create the container and return a container client object
//    BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

//    // Create the container if it does not exist
//    await containerClient.CreateIfNotExistsAsync();

//    // Get a reference to a blob
//    BlobClient blobClient = containerClient.GetBlobClient(blobName);

//    Console.WriteLine($"Uploading to Blob storage as blob:\n\t {blobClient.Uri}");

//    // Open the file and upload its data
//    using FileStream uploadFileStream = File.OpenRead(filePath);
//    await blobClient.UploadAsync(uploadFileStream, true);
//    uploadFileStream.Close();

//    Console.WriteLine("Upload complete");
//}