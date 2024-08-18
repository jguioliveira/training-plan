using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Infrastructure.EntitiesConfig;

namespace TrainingPlan.Infrastructure.DbContext
{
    public class WorkoutEntityTypeConfiguration : EntityMapBase<Workout>
    {
        public override void Configure(EntityTypeBuilder<Workout> builder)
        {
            base.Configure(builder);

            builder
                .Property(b => b.Date)
                .HasColumnType("date")
                .IsRequired();

            builder
                .Property(b => b.Description)
                .HasColumnType("text");

            builder
                .HasOne(b => b.Content)
                .WithMany()
                .HasForeignKey(b => b.ContentId)
                .IsRequired(false);
        }

    }
}
