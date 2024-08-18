

namespace TrainingPlan.Domain.Entities
{
    public class Team(string name, string email) : BaseEntity()
    {
        public string Name { get; private set; } = name;

        public string Email { get; private set; } = email;

        public virtual IReadOnlyCollection<TeamSettings>? TeamSettings { get; private set; }

        public virtual IReadOnlyCollection<SocialMedia>? SocialsMedia { get; private set; }


        public void UpdateName(string name)
        {
            Name = name;
        }

        public void UpdateEmail(string email)
        {
            Email = email;
        }
    }

    public class SocialMedia(int id, int teamId, string name, string account)
    {
        public int Id { get; private set; } = id;
        public int TeamId { get; private set; } = teamId;
        public string Name { get; private set; } = name;
        public string Account { get; private set; } = account;

        public virtual Team Team { get; private set; }
    }

    public class TeamSettings(int teamId, string key, string value)
    {
        public int TeamId { get; private set; } = teamId;

        public string Key { get; private set; } = key;

        public string Value { get; private set; } = value;

        public virtual Team Team { get; private set; }
    }
}
