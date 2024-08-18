using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainingPlan.Domain.Entities;

namespace TrainingPlan.Infrastructure.DbContext
{
    public class TeamSettingsEntityTypeConfiguration : IEntityTypeConfiguration<TeamSettings>
    {
        public void Configure(EntityTypeBuilder<TeamSettings> builder)
        {
            builder.HasKey(t => new { t.Key, t.TeamId });

            builder
                .Property(b => b.TeamId)
                .IsRequired();

            builder
                .Property(b => b.Key)
                .HasColumnType("text")
                .IsRequired();

            builder
                .Property(b => b.Value)
                .HasColumnType("text")
                .IsRequired();
        }

    }
}
