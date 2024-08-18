using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Infrastructure.EntitiesConfig;

namespace TrainingPlan.Infrastructure.DbContext
{
    public class PersonEntityTypeConfiguration : EntityMapBase<Person>
    {
        public override void Configure(EntityTypeBuilder<Person> builder)
        {
            base.Configure(builder);

            builder
                .Property(b => b.UserId)
                .IsRequired();

            builder
                .Property(b => b.Email)
                .HasColumnType("text")
                .IsRequired();

            builder
                .Property(b => b.Name)
                .HasColumnType("text")
                .IsRequired();
            
            builder
                .Property(b => b.Type)
                .HasColumnType("text")
                .IsRequired();

            builder
                .Property(b => b.Phone)
                .HasColumnType("text")
                .IsRequired();

            builder.Property(b => b.Birth)
                .HasColumnType("date")
                .IsRequired();

            builder
                .HasMany(p => p.TrackingServices)
                .WithOne(p => p.Person)
                .HasForeignKey(t => t.PersonId)
                .IsRequired();
        }
    }
}
