using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;
using TrainingPlan.Infrastructure.DbContext;

namespace TrainingPlan.Infrastructure.Repositories
{
    public class ContentRepository(ApplicationDbContext dbContext) : BaseRepository<Content>(dbContext), IContentRepository
    {
    }
}
