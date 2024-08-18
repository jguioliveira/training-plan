namespace TrainingPlan.Domain.Entities
{
    public class Activity
    {
        public Guid Id { get; set; }
        public string? ImportedFrom { get; set; }
        public string? ImportedData { get; set; }

        public DateTime? Time { get; set; }
        public double? Distance { get; set; }
        public string? DistanceUnit { get; set; }

        public DateTime? AveragePace { get; set; }
        public string? AveragePaceUnit { get; set; }
        public int? AverageHeartHate { get; set; }
    }
}
