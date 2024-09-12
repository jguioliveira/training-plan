
namespace TrainingPlan.Domain.DTO
{
    public record TeamDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public IEnumerable<TeamSettingsDTO>? TeamSettings { get; set; }

        public IEnumerable<SocialMediaDTO>? SocialsMedia { get; set; }
    }

    public record TeamSettingsDTO
    {
        public string Key { get; set; }

        public string Value { get; set; }

    }
    public record SocialMediaDTO
    {
        public string Name { get; set; }
        public string Account { get; set; }
        
    }

}
