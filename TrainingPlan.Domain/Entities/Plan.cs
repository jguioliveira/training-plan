
namespace TrainingPlan.Domain.Entities
{
    public class Plan(string name, string goal, int athleteId, int instructorId, string? description = null) : BaseEntity()
    {
        public string Name { get; private set; } = name;
        public string? Description { get; set; } = description;
        public string Goal { get; private set; } = goal;
        public int AthleteId { get; private set; } = athleteId;
        public int InstructorId { get; private set; } = instructorId;

        public virtual Athlete Athlete { get; private set; }

        public virtual Instructor Instructor { get; private set; }

        private List<Workout> _workouts = [];
        public virtual IReadOnlyCollection<Workout>? Workouts 
        { 
            get { return _workouts; } 
        }

        public void UpdateInstructor(Instructor instructor)
        {
            InstructorId = instructor.Id;
            Instructor = instructor;
        }

        public void UpdateName(string name)
        {
            Name = name;
        }

        public void UpdateGoal(string goal)
        {
            Goal = goal;
        }

        public void UpdateDescription(string? description)
        {
            Description = description;
        }
    }
}
