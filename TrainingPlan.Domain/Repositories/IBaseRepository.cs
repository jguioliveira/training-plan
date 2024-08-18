

using TrainingPlan.Domain.Entities;

namespace TrainingPlan.Domain.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity?> GetAsync(int id, CancellationToken cancellationToken);

        void Create(TEntity entity);

        void Update(TEntity entity);
    }
}
