namespace QuizMaker.Data
{
    using Microsoft.EntityFrameworkCore;
    using QuizMaker.Models;
    using System.Collections.Generic;
    using System.Reflection.Emit;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<QuizResult> QuizResults { get; set; }
        public DbSet<QuizResultAnswer> QuizResultAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Quiz>()
                .HasMany(q => q.Questions)
                .WithMany(q => q.Quizzes)
                .UsingEntity(j => j.ToTable("QuizQuestions"));

            modelBuilder.Entity<Quiz>()
                .Property(q => q.Title)
                .IsRequired();

            modelBuilder.Entity<Question>()
                .Property(q => q.Text)
                .IsRequired();

            modelBuilder.Entity<Answer>()
                .Property(q => q.Text)
                .IsRequired();

            modelBuilder.Entity<Question>()
                .HasMany(q => q.Answers)
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Quiz>()
                .HasOne(q => q.CreatedBy)
                .WithMany()
                .HasForeignKey(q => q.CreatedById)
                .OnDelete(DeleteBehavior.SetNull); //if the User is deleted, the CreatedById column in Quiz is set to NULL

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired(false);

            modelBuilder.Entity<QuizResultAnswer>(entity =>
            {
                entity.HasOne(qra => qra.QuizResult)
                    .WithMany(qr => qr.Answers)
                    .HasForeignKey(qra => qra.QuizResultId)
                    .OnDelete(DeleteBehavior.Cascade); // safe

                entity.HasOne(qra => qra.Question)
                    .WithMany()
                    .HasForeignKey(qra => qra.QuestionId)
                    .OnDelete(DeleteBehavior.Restrict); // prevent

                entity.HasOne(qra => qra.Answer)
                    .WithMany()
                    .HasForeignKey(qra => qra.AnswerId)
                    .OnDelete(DeleteBehavior.Restrict); // prevent
            });

        }
    }

}
