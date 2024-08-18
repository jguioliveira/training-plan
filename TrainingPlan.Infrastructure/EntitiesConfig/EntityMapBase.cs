using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using TrainingPlan.Domain.Entities;

namespace TrainingPlan.Infrastructure.EntitiesConfig
{
    public interface IEntityMap<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
    {
    }

    public abstract class EntityMapBase<TEntity> : IEntityMap<TEntity> where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder
                .Property(b => b.Id)
                .IsRequired()
                .UseIdentityColumn();
        }
    }
}
