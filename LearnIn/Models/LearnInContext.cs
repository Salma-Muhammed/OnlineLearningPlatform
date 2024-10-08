using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LearnIn.Models;
using System.Reflection.Emit;

namespace LearnIn.Data
{
    public class LearnInContext : IdentityDbContext<ApplicationUser>
    {
        public LearnInContext(DbContextOptions<LearnInContext> options) : base(options){}
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enroll> Enrolls { get; set; }
        public DbSet<Teach> Teaches { get; set; }
        public DbSet<CourseCategory> CourseCategories { get; set; }
        public DbSet<StudentTakesExam> StudentTakesExams { get; set; }
        public DbSet<StudentAnswer> StudentAnswers { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> questions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Enroll: Composite key
            builder.Entity<Enroll>()
                .HasKey(e => new { e.UserId, e.CourseId });

            // Teach: Composite key
            builder.Entity<Teach>()
                .HasKey(t => new { t.UserId, t.CourseId });

            // CourseCategory: Composite key
            builder.Entity<CourseCategory>()
                .HasKey(cc => new { cc.CategoryId, cc.CourseId });

            // StudentTakesExam: Composite key
            builder.Entity<StudentTakesExam>()
                .HasKey(ste => new { ste.UserId, ste.ExamId });

            // StudentAnswer: Composite key
            builder.Entity<StudentAnswer>()
                .HasKey(sa => new { sa.UserId, sa.QuestionId });

            // CourseTopic: Composite key
            builder.Entity<CourseTopic>()
                .HasKey(ct => new { ct.CourseId, ct.TopicId });

            builder.Entity<CourseTopic>()
                .HasOne(ct => ct.Course)
                .WithMany(c => c.CourseTopics)
                .HasForeignKey(ct => ct.CourseId);

            builder.Entity<CourseTopic>()
                .HasOne(ct => ct.Topic)
                .WithMany(t => t.CourseTopics)
                .HasForeignKey(ct => ct.TopicId);

            //Preventing EF From creating ApplicationUserId and Ignore UserId
            builder.Entity<StudentTakesExam>()
                .HasOne(e => e.ApplicationUser)
                .WithMany(a => a.StudentTakesExams)
                .HasForeignKey(e => e.UserId)
                .IsRequired(false); // If UserId is nullable, else remove this

            builder.Entity<Enroll>()
                .HasOne(e => e.ApplicationUser)
                .WithMany(a => a.Enrolls)
                .HasForeignKey(e => e.UserId);

             builder.Entity<StudentAnswer>()
                .HasOne(e => e.ApplicationUser)
                .WithMany(a => a.StudentAnswers)
                .HasForeignKey(e => e.UserId);

             builder.Entity<Teach>()
                .HasOne(e => e.ApplicationUser)
                .WithMany(a => a.Teaches)
                .HasForeignKey(e => e.UserId);
        }

    }
}
