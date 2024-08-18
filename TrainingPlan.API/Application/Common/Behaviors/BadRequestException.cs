namespace TrainingPlan.API.Application.Common.Behaviors
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {
        }

        public BadRequestException(string[] errors) : base("Multiple errors occurred. See error details.")
        {
            Errors = errors;
        }

        public string[] Errors { get; set; }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
}
