using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GroupC.Uni.Core;
using GroupC.Uni.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace GroupC.Uni.Infrastructure
{
    public class AppDbContext :
        IdentityDbContext< ApplicationUser, IdentityRole<Guid>, Guid> 
        
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {

        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<Choice> Choices { get; set; }
        public virtual DbSet<Exam> Exams { get; set; }

        public virtual DbSet<ExamQuestion> ExamQuestions { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        // public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Submission> Submissions { get; set; }
        public virtual DbSet<SubmissionChoice> SubmissionChoices { get; set; }
        public virtual DbSet<TestCenter> TestCenters { get; set; }
        public virtual DbSet<Topic> Topics { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Question>()
            .HasMany(q => q.Choices)
            .WithOne(x => x.Question)
            .HasForeignKey(x => x.QuestionId)
            .IsRequired(true).OnDelete(DeleteBehavior.Restrict);

            

            builder.Entity<ExamQuestion>()
                .HasOne(bc => bc.Exam)
                .WithMany(b => b.ExamQuestions)
                .HasForeignKey(bc => bc.ExamId).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ExamQuestion>()
                .HasOne(bc => bc.Question)
                .WithMany(c => c.ExamQuestions)
                .HasForeignKey(bc => bc.QuestionId).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Topic>()
          .HasMany(q => q.Questions)
          .WithOne(x => x.Topic)
          .HasForeignKey(x => x.TopicId)
          .IsRequired(true).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Course>()
          .HasMany(q => q.Topics)
          .WithOne(x => x.Course)
          .HasForeignKey(x => x.CourseId)
          .IsRequired(true).OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<SubmissionChoice>()
                .HasOne(bc => bc.Submission)
                .WithMany(b => b.SubmissionChoices)
                .HasForeignKey(bc => bc.SubmissionId).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SubmissionChoice>()
                .HasOne(bc => bc.Choice)
                .WithMany(c => c.SubmissionChoices)
                .HasForeignKey(bc => bc.ChoiceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Submission>()
     .HasMany(q => q.SubmissionChoices)
     .WithOne(x => x.Submission)
     .HasForeignKey(x => x.SubmissionId)
     .IsRequired(true).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Exam>()
     .HasMany(q => q.ExamQuestions)
     .WithOne(x => x.Exam)
     .HasForeignKey(x => x.ExamId)
     .IsRequired(true).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Exam>()
     .HasMany(q => q.Submissions)
     .WithOne(x => x.Exam)
     .HasForeignKey(x => x.ExamId)
     .IsRequired(true).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Course>()
    .HasMany(q => q.Exams)
    .WithOne(x => x.Course)
    .HasForeignKey(x => x.CourseId)
    .IsRequired(true).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TestCenter>()
    .HasMany(q => q.Exams)
    .WithOne(x => x.TestCenter)
    .HasForeignKey(x => x.TestCenterId)
    .IsRequired(true).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Student>()
    .HasMany(q => q.Submissions)
    .WithOne(x => x.Student)
    .HasForeignKey(x => x.StudentId)
    .IsRequired(true).OnDelete(DeleteBehavior.Restrict);


            builder.Entity<Admin>()
                .HasOne(a => a.ApplicationUser)
                .WithOne(u => u.Admin)
                .HasForeignKey<Admin>(a => a.Id);
            builder.Entity<Student>()
                .HasOne(a => a.ApplicationUser)
                .WithOne(u => u.Student)
                .HasForeignKey<Student>(a => a.Id);
            builder.Entity<TestCenter>()
                .HasOne(a => a.ApplicationUser)
                .WithOne(u => u.TestCenter)
                .HasForeignKey<TestCenter>(a => a.Id);
            base.OnModelCreating(builder);
        }
    }
}
