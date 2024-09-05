using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Infrastructure.EntitiesConfig;

namespace TrainingPlan.Infrastructure.DbContext
{
    public class CommentEntityTypeConfiguration : EntityMapBase<Comment>
    {
        public override void Configure(EntityTypeBuilder<Comment> builder)
        {
            base.Configure(builder);

            builder
                .Property(b => b.Text)
                .HasColumnType("text")
                .IsRequired();

            builder
                .Property(b => b.PersonType)
                .HasColumnType("text")
                .IsRequired();

            builder
                .Property(b => b.PersonName)
                .HasColumnType("text")
                .IsRequired();

            builder
                .HasOne(b => b.Workout)
                .WithMany(b => b.Comments)
                .HasForeignKey(b => b.WorkoutId)
                .IsRequired();

        }

    }
}
