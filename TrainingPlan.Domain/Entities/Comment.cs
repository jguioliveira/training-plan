
namespace TrainingPlan.Domain.Entities
{
    public class Comment(int personId, string personName, string personType, string text, int workoutId) : BaseEntity()
    {
        public int PersonId { get; private set; } = personId;
        public string PersonName { get; private set; } = personName;
        public string PersonType { get; private set; } = personType;
        public string Text { get; private set; } = text;

        public int WorkoutId { get; private set; } = workoutId;

        public bool IsRemoved { get; private set; } = false;

        public virtual Workout Workout { get; set; }

        public void UpdateText(string text)
        {
            Text = text;
        }

        public void Remove()
        {
            IsRemoved = true;
        }
    }
}
