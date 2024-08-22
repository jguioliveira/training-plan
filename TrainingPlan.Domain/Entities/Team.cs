

namespace TrainingPlan.Domain.Entities
{
    public class Team(string name, string email) : BaseEntity()
    {
        bool isTeamSettingsReset;

        public string Name { get; private set; } = name;

        public string Email { get; private set; } = email;

        private List<TeamSettings> _teamSettings = [];
        public virtual IReadOnlyCollection<TeamSettings> TeamSettings
        {
            get
            {
                return _teamSettings;
            }
        }

        private List<SocialMedia> _socialMedia = [];
        public virtual IReadOnlyCollection<SocialMedia>? SocialsMedia
        {
            get
            {
                return _socialMedia;
            }
        }


        public void UpdateName(string name)
        {
            Name = name;
        }

        public void UpdateEmail(string email)
        {
            Email = email;
        }

        public void AddSettings(string key, string value)
        {
            if (!isTeamSettingsReset)
            {
                _teamSettings = [];
                isTeamSettingsReset = true;
            }

            TeamSettings settings = new(Id, key, value);
            _teamSettings.Add(settings);
        }

        public void SaveSocialMedia(string name, string account)
        {
            _socialMedia ??= [];

            var socialMedia = _socialMedia.SingleOrDefault(s => s.Name == name);

            if (socialMedia != null)
            {
                //when the social media is found, we only update the account information
                socialMedia.Account = account;
            }
            else
            {
                socialMedia = new(Id, name, account);
                _socialMedia.Add(socialMedia);
            }
        }

        public void DeleteSocialMedia(string name)
        {
            _socialMedia ??= [];

            var socialMedia = _socialMedia.SingleOrDefault(s => s.Name == name);

            if (socialMedia != null)
            {
                _socialMedia.Remove(socialMedia);
            }
        }
    }

    public class SocialMedia(int teamId, string name, string account)
    {
        public int TeamId { get; private set; } = teamId;
        public string Name { get; private set; } = name;
        public string Account { get; internal set; } = account;

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
