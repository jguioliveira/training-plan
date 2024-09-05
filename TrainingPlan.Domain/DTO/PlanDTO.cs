
namespace TrainingPlan.Domain.DTO
{
    public record PlanDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public string Goal { get; set; }
        public int InstructorId { get; set; }

        public InstructorDTO Instructor { get; set; }

        public IEnumerable<WorkoutDTO>? Workouts { get; set; }
    }

    public record WorkoutDTO
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int? ContentId { get; set; }
        public ContentDTO? Content { get; set; }
        public IEnumerable<CommentDTO>? Comments { get; set; }
    }

    public record ContentsPagedListDTO : PaginationResult<ContentDTO>
    {
    }

    public record ContentDTO
    {
        public string Title { get; set; }

        public string? Description { get; set; }
        public string Data { get; set; }

        public string Type { get; set; }
    }

    public record CommentDTO
    {
        public int Id { get; set; }
        public string PersonName { get; set; }
        public string PersonType { get; set; }
        public string Text { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}