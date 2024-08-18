namespace TrainingPlan.Domain.Entities
{
    public class Comment(string personName, string personType, string text, int workoutId) : BaseEntity()
    {
        public string PersonName { get; private set; } = personName;
        public string PersonType { get; private set; } = personType;
        public string Text { get; private set; } = text;
        public DateTime Date { get; private set; }

        public int WorkoutId { get; private set; } = workoutId;

        public virtual Workout Workout { get; set; }

    }
}
