namespace TrainingPlan.Domain.Entities
{
    public class Person(Guid userId, string name, DateTime birth, string email, string phone, string type) : BaseEntity()
    {
        public Guid UserId { get; private set; } = userId;
        public string Name { get; private set; } = name;
        public DateTime Birth { get; private set; } = birth;
        public string Email { get; private set; } = email;
        public string Phone { get; private set; } = phone;
        public string Type { get; private set; } = type;

        /// <summary>
        /// Tracking physical exercise service (Garmin, Strava, Polar)
        /// </summary>
        public virtual ICollection<TrackingService>? TrackingServices { get; private set; }

        public virtual IReadOnlyCollection<Plan>? Plans { get; private set; }

        public void UpdateEmail(string email)
        {
            Email = email;
        }

        public void UpdateName(string name)
        {
            Name = name;
        }

        public void UpdatePhone(string phone)
        {
            Phone = phone;
        }

        public void UpdateBirth(DateTime birth)
        {
            Birth = birth;
        }

    }

    public class Athlete(Guid userId, string name, DateTime birth, string email, string phone) 
        : Person(userId, name, birth, email, phone, PersonType.ATHLETE)
    {
    }

    public class Instructor(Guid userId, string name, DateTime birth, string email, string phone) 
        : Person(userId, name, birth, email, phone, PersonType.INSTRUCTOR)
    {
    }

    public static class PersonType
    {
        public const string ATHLETE = "ATHLETE";
        public const string INSTRUCTOR = "INSTRUCTOR";
    }
}
