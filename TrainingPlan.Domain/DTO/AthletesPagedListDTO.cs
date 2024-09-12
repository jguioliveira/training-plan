namespace TrainingPlan.Domain.DTO
{

    public record AthletesPagedListDTO : PaginationResult<AthleteDTO>
    {
    }

    public record InstructorsPagedListDTO : PaginationResult<InstructorDTO>
    {
    }
}
