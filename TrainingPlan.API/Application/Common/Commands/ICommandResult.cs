namespace TrainingPlan.API.Application.Common.Commands
{
    public interface ICommandResult
    {
        bool Success { get; set; }
        object Message { get; set; }
    }
}
