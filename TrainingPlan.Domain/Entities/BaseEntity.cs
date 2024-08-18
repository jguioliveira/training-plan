namespace TrainingPlan.Domain.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
        }

        public int Id { get; private set; }

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public DateTime ModifiedAt { get; private set; } = DateTime.UtcNow;
    }
}
