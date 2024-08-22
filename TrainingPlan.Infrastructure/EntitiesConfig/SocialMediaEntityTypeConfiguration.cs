using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainingPlan.Domain.Entities;

namespace TrainingPlan.Infrastructure.DbContext
{
    public class SocialMediaEntityTypeConfiguration : IEntityTypeConfiguration<SocialMedia>
    {
        public void Configure(EntityTypeBuilder<SocialMedia> builder)
        {
            builder.HasKey(t => new { t.Name, t.TeamId });

            builder
                .Property(b => b.TeamId)
                .IsRequired();

            builder
                .Property(b => b.Name)
                .HasColumnType("text")
                .IsRequired();

            builder
                .Property(b => b.Account)
                .HasColumnType("text")
                .IsRequired();
        }

    }
}
