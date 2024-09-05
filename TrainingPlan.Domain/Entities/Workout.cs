namespace TrainingPlan.Domain.Entities
{
    public class Workout(DateTime date, string description, int planId, int? contentId = null) : BaseEntity()
    {
        public DateTime Date { get; private set; } = date;
        public string Description { get; private set; } = description;
        public int PlanId { get; private set; } = planId;
        public int? ContentId { get; private set; }

        public virtual Plan Plan { get; private set; }
        public virtual Content? Content { get; private set; }

        private readonly List<Comment> _comments = new();
        public virtual IReadOnlyCollection<Comment>? Comments 
        { 
            get { return _comments; }
        }

        public void UpdateContent(Content content)
        {
            contentId = content.Id;
            Content = content;
        }

        public void UpdateDate(DateTime date)
        {
            Date = date;
        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }

        public void AddComment(Comment comment)
        {
            _comments.Add(comment);
        }
    }
}
