
namespace TrainingPlan.Domain.Entities
{
    public class TrackingService(int personId, string name, string connectionData)
    {
        public int PersonId { get; private set; } = personId;
        public string Name { get; private set; } = name;
        public string ConnectionData { get; private set; } = connectionData;

        public virtual Person Person { get; private set; }
    }
}
