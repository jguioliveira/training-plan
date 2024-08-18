
namespace TrainingPlan.Domain.DTO
{
    

    public record PaginationResult<IResponseDTO> //: IPaginationResponse<IResponseDTO>
    {
        public int Total { get; set; }
        public IEnumerable<IResponseDTO> Items { get; set; }
    }
}
