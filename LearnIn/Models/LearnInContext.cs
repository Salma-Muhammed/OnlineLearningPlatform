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
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enroll> Enrolls { get; set; }
        public DbSet<Teach> Teaches { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<Topic> Topics{ get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Enroll: Composite key
            builder.Entity<Enroll>()
                .HasKey(e => new { e.UserId, e.CourseId });

            // Teach: Composite key
            builder.Entity<Teach>()
                .HasKey(t => new { t.UserId, t.CourseId });                 

            builder.Entity<Enroll>()
                .HasOne(e => e.ApplicationUser)
                .WithMany(a => a.Enrolls)
                .HasForeignKey(e => e.UserId);
           
             builder.Entity<Teach>()
                .HasOne(e => e.ApplicationUser)
                .WithMany(a => a.Teaches)
                .HasForeignKey(e => e.UserId);

            builder.Entity<ContactUs>()
                .HasOne(c => c.ApplicationUser) // Assuming one user per message
                .WithMany()                     // No navigation property in ApplicationUser
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);  // If a user is deleted, delete their messages

            builder.Entity<Topic>()
                .HasMany(t => t.Contents)
                .WithOne(c => c.Topic)
                .HasForeignKey(c => c.TopicId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Content>()
               .HasOne(c => c.Topic)
               .WithMany(t => t.Contents)
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
