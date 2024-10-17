using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LearnIn.Models;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;

namespace LearnIn.Data
{
    public class LearnInContext : IdentityDbContext<ApplicationUser>
    {
        public LearnInContext(DbContextOptions<LearnInContext> options) : base(options){}
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enroll> Enrolls { get; set; }
        public DbSet<TopicContent> TopicContents { get; set; }
        public DbSet<Topic> Topics{ get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Enroll: Composite key
            builder.Entity<Enroll>()
                .HasKey(e => new { e.UserId, e.CourseId });

            // Relationships
            builder.Entity<Course>()
                .HasOne(c => c.ApplicationUser)
                .WithMany(u => u.Courses)
                .HasForeignKey(c => c.UserId);

            builder.Entity<Enroll>()
                .HasOne(e => e.ApplicationUser)
                .WithMany(u => u.Enrolls)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Enroll>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrolls)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<Topic>()
                .HasMany(t => t.TopicContents)
                .WithOne(c => c.Topic)
                .HasForeignKey(c => c.TopicId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TopicContent>()
               .HasOne(c => c.Topic)
               .WithMany(t => t.TopicContents)
               .HasForeignKey(c => c.TopicId)
               .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "Student", NormalizedName = "STUDENT" },
                new IdentityRole { Name = "Instructor", NormalizedName = "INSTRUCTOR" }
            );
        }
       
    }
}
