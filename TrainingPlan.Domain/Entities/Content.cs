

namespace TrainingPlan.Domain.Entities
{
    public class Content(string title, string data, string type) : BaseEntity()
    {
        public string Title { get; private set; } = title;

        public string? Description { get; set; }

        public string Data { get; private set; } = data;

        public string Type { get; private set; } = type;

        public void UpdateData(string data, string type)
        {
            Data = data;
            Type = type;
        }

        public void UpdateTitle(string title)
        {
            Title = title;
        }
    }
}
