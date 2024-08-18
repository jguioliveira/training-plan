
namespace TrainingPlan.Domain.Entities
{
    public class Content(string title, string blobId, string type) : BaseEntity()
    {
        public string Title { get; private set; } = title;

        public string? Description { get; set; }
        public string BlobId { get; private set; } = blobId;

        public string Type { get; private set; } = type;

    }
}
