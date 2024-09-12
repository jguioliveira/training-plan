using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;
using TrainingPlan.Infrastructure.DbContext;

namespace TrainingPlan.Infrastructure.Repositories
{
    public class ContentRepository(ApplicationDbContext dbContext) : BaseRepository<Content>(dbContext), IContentRepository
    {
        public Task<ContentDTO?> GetContentAsync(int id)
        {
            string query = "SELECT * FROM \"Content\" WHERE \"Id\" = @param;";

            return GetAsync<ContentDTO>(query, id);
        }

        public async Task<ContentsPagedListDTO> GetContentAsync(int lastId, int pageSize, string direction, string title)
        {
            var filters = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(title))
            {
                filters.Add("Title", title);
            }

            var results = await base.GetPagedAsync("Contents", lastId, pageSize, direction, filters);

            var items = await results.ReadAsync<ContentDTO>();
            var total = await results.ReadSingleAsync<int>();

            var contents = new ContentsPagedListDTO
            {
                Items = items,
                Total = total,
            };

            return contents;
        }
    }
}
