using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Infrastructure.EntitiesConfig;

namespace TrainingPlan.Infrastructure.DbContext
{
    public class PlanEntityTypeConfiguration : EntityMapBase<Plan>
    {
        public override void Configure(EntityTypeBuilder<Plan> builder)
        {
            base.Configure(builder);

            builder
                .Property(b => b.Name)
                .HasColumnType("text")
                .IsRequired();

            builder
                .Property(b => b.Goal)
                .HasColumnType("text")
                .IsRequired();

            builder
                .Property(b => b.Description)
                .HasColumnType("text");

            builder
                .HasOne(b => b.Athlete)
                .WithMany(b => b.Plans)
                .HasForeignKey(b => b.AthleteId);

            builder
                .HasOne(b => b.Instructor)
                .WithMany(b => b.Plans)
                .HasForeignKey(b => b.InstructorId);

            builder
                .HasMany(b => b.Workouts)
                .WithOne(b => b.Plan)
                .HasForeignKey(b => b.PlanId)
                .IsRequired();
        }

    }
}
