using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Infrastructure.EntitiesConfig;

namespace TrainingPlan.Infrastructure.DbContext
{
    public class ContentEntityTypeConfiguration : EntityMapBase<Content>
    {
        public override void Configure(EntityTypeBuilder<Content> builder)
        {
            base.Configure(builder);

            builder
                .Property(b => b.Title)
                .HasColumnType("text")
                .IsRequired();

            builder
                .Property(b => b.Description)
                .HasColumnType("text");

            builder
                .Property(b => b.BlobId)
                .HasColumnType("text")
                .IsRequired();

            builder
                .Property(b => b.Type)
                .HasColumnType("text")
                .IsRequired();

        }

    }
}
