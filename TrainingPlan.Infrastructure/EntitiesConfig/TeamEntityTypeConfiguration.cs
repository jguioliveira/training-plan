using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Infrastructure.EntitiesConfig;

namespace TrainingPlan.Infrastructure.DbContext
{
    public class TeamEntityTypeConfiguration : EntityMapBase<Team>
    {
        public override void Configure(EntityTypeBuilder<Team> builder)
        {
            base.Configure(builder);

            builder
                .Property(b => b.Email)
                .HasColumnType("text")
                .IsRequired();

            builder
                .Property(b => b.Name)
                .HasColumnType("text")
                .IsRequired();

            builder
                .HasMany(t => t.TeamSettings)
                .WithOne(s => s.Team)
                .HasForeignKey(t => t.TeamId)
                .IsRequired();

            builder
                .HasMany(t => t.SocialsMedia)
                .WithOne(s => s.Team)
                .HasForeignKey(t => t.TeamId)
                .IsRequired();
        }

    }
}
