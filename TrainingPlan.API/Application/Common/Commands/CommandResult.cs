namespace TrainingPlan.API.Application.Common.Commands
{
    public record CommandResult : ICommandResult
    {
        public CommandResult(bool success, object message, IDictionary<string, string[]>? errors)
        {
            Success = success;
            Message = message;
            Errors = errors;
        }

        public bool Success { get; set; }
        public object Message { get; set; }
        public IDictionary<string, string[]>? Errors { get; set; }
    }
}
