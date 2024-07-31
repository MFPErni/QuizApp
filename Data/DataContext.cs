// Data/DataContext.cs
using Microsoft.EntityFrameworkCore;
using IntroBE.Entities;

namespace IntroBE.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Admin> AdminList { get; set; }
        public DbSet<Guest> GuestList { get; set; }
        public DbSet<Quiz> QuizList { get; set; }
        public DbSet<Question> QuestionList { get; set; }
        public DbSet<Answer> AnswerList { get; set; }
        public DbSet<GuestQuizScore> GuestQuizScoreList { get; set; }
        public DbSet<Category> CategoryList { get; set; } // Add this line

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure one-to-many relationships
            modelBuilder.Entity<Quiz>()
                .HasOne(q => q.Admin)
                .WithMany(a => a.Quizzes)
                .HasForeignKey(q => q.AdminID);

            modelBuilder.Entity<Question>()
                .HasOne(q => q.Quiz)
                .WithMany(qz => qz.Questions)
                .HasForeignKey(q => q.QuizID);

            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionID);

            modelBuilder.Entity<GuestQuizScore>()
                .HasOne(gqs => gqs.Guest)
                .WithMany(g => g.GuestQuizScores)
                .HasForeignKey(gqs => gqs.GuestID);

            modelBuilder.Entity<GuestQuizScore>()
                .HasOne(gqs => gqs.Quiz)
                .WithMany(q => q.GuestQuizScores)
                .HasForeignKey(gqs => gqs.QuizID);

            modelBuilder.Entity<Quiz>()
                .HasOne(q => q.Category)
                .WithMany(c => c.Quizzes)
                .HasForeignKey(q => q.CategoryID); // Add this line
        }
    }
}