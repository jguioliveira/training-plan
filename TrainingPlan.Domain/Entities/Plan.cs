namespace TrainingPlan.Domain.Entities
{
    public class Plan(string name, string goal, int ahtleteId, int instructorId) : BaseEntity()
    {
        public string Name { get; private set; } = name;
        public string? Description { get; set; }
        public string Goal { get; private set; } = goal;
        public int AhtleteId { get; private set; } = ahtleteId;
        public int InstructorId { get; private set; } = instructorId;

        public virtual Athlete Athlete { get; private set; }

        public virtual Instructor Instructor { get; private set; }

        public virtual IReadOnlyCollection<Workout>? Workouts { get; private set; }
    }
}
