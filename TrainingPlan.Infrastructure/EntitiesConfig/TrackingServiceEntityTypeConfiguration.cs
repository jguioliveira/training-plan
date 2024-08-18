using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainingPlan.Domain.Entities;

namespace TrainingPlan.Infrastructure.DbContext
{
    public class TrackingServiceEntityTypeConfiguration : IEntityTypeConfiguration<TrackingService>
    {
        public void Configure(EntityTypeBuilder<TrackingService> builder)
        {
            builder.HasKey(t => new { t.PersonId, t.Name });

            builder
                .Property(b => b.PersonId)
                .IsRequired();

            builder
                .Property(b => b.Name)
                .HasColumnType("text")
                .IsRequired();

            builder
                .Property(b => b.ConnectionData)
                .HasColumnType("text")
                .IsRequired();
        }
    }
}
