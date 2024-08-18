using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Shared.User;

namespace TrainingPlan.Infrastructure.DbContext
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Team> Teams { get; set; }

        public DbSet<Person> Person { get; set; }
        public DbSet<TrackingService> TrackingServices { get; set; }

        public DbSet<Plan> Plans { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Comment> Comments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TeamEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TeamSettingsEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SocialMediaEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new PersonEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TrackingServiceEntityTypeConfiguration());


            modelBuilder.ApplyConfiguration(new PlanEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new WorkoutEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ContentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CommentEntityTypeConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
