namespace TrainingPlan.Domain.Entities
{
    public class Workout(DateTime date, string description, int planId) : BaseEntity()
    {
        public DateTime Date { get; private set; } = date;
        public string Description { get; private set; } = description;
        public int PlanId { get; private set; } = planId;
        public int? ContentId { get; private set; }

        public virtual Plan Plan { get; private set; }
        public virtual Content? Content { get; private set; }
        public virtual IReadOnlyCollection<Comment>? Comments { get; private set; }
    }
}
