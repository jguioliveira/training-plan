namespace TrainingPlan.Domain.DTO
{
    public record AthleteDTO
    {
        public int Id { get; set; }
        public Guid UserId { get;  set; } 
        public string Name { get;  set; }
        public DateTime Birth { get;  set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Type { get;  set; }
    }
}
