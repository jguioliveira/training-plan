using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Entities;

namespace TrainingPlan.Domain.Repositories
{
    public interface IContentRepository : IBaseRepository<Content>
    {
        Task<ContentDTO?> GetContentAsync(int id);

        Task<ContentsPagedListDTO> GetContentAsync(int lastId, int pageSize, string direction, string title);
    }
}
