using QuizMaker.Models;
using Microsoft.EntityFrameworkCore;
using QuizMaker.Authentication;

namespace QuizMaker.Data
{
    public static class DummyDataSeeder
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            if (await db.Users.AnyAsync()) return; // Prevent double seeding

            // --- Users ---
            var admin = new User { Email = "admin@example.com", NickName = "AdminUser", Role = UserRole.Administrator, PasswordHash = PasswordHasher.GetHash("123456") };
            var superUser = new User { Email = "superuser@example.com", NickName = "SuperUser", Role = UserRole.SuperUser, PasswordHash = PasswordHasher.GetHash("123456") };
            var normalUser = new User { Email = "user@example.com", NickName = "NormalUser", PasswordHash = PasswordHasher.GetHash("123456") };

            db.Users.AddRange(admin, superUser, normalUser);
            await db.SaveChangesAsync();

            // --- Predefined Questions ---
            var predefinedQuestions = new List<Question>
            {
                new Question
                {
                    Text = "What is the capital of France?",
                    Answers = new List<Answer>
                    {
                        new Answer { Text = "Paris", IsCorrect = true },
                        new Answer { Text = "Berlin", IsCorrect = false },
                        new Answer { Text = "Rome", IsCorrect = false },
                        new Answer { Text = "Madrid", IsCorrect = false }
                    }
                },
                new Question
                {
                    Text = "Which planet is known as the Red Planet?",
                    Answers = new List<Answer>
                    {
                        new Answer { Text = "Venus", IsCorrect = false },
                        new Answer { Text = "Mars", IsCorrect = true },
                        new Answer { Text = "Jupiter", IsCorrect = false },
                        new Answer { Text = "Saturn", IsCorrect = false }
                    }
                },
                new Question
                {
                    Text = "What is 5 + 7?",
                    Answers = new List<Answer>
                    {
                        new Answer { Text = "10", IsCorrect = false },
                        new Answer { Text = "12", IsCorrect = true },
                        new Answer { Text = "14", IsCorrect = false },
                        new Answer { Text = "11", IsCorrect = false }
                    }
                },
                new Question
                {
                    Text = "Who wrote 'Hamlet'?",
                    Answers = new List<Answer>
                    {
                        new Answer { Text = "Charles Dickens", IsCorrect = false },
                        new Answer { Text = "Leo Tolstoy", IsCorrect = false },
                        new Answer { Text = "Mark Twain", IsCorrect = false },
                        new Answer { Text = "William Shakespeare", IsCorrect = true },
                    }
                },
                new Question
                {
                    Text = "Which ocean is the largest?",
                    Answers = new List<Answer>
                    {
                        new Answer { Text = "Pacific Ocean", IsCorrect = true },
                        new Answer { Text = "Atlantic Ocean", IsCorrect = false },
                        new Answer { Text = "Indian Ocean", IsCorrect = false },
                        new Answer { Text = "Arctic Ocean", IsCorrect = false }
                    }
                },
                new Question
                {
                    Text = "Which country hosted the 2016 Summer Olympics?",
                    Answers = new List<Answer>
                    {
                        new Answer { Text = "China", IsCorrect = false },
                        new Answer { Text = "UK", IsCorrect = false },
                        new Answer { Text = "Russia", IsCorrect = false },
                        new Answer { Text = "Brazil", IsCorrect = true },
                    }
                },
                new Question
                {
                    Text = "What is the chemical symbol for water?",
                    Answers = new List<Answer>
                    {
                        new Answer { Text = "O2", IsCorrect = false },
                        new Answer { Text = "H2O", IsCorrect = true },
                        new Answer { Text = "CO2", IsCorrect = false },
                        new Answer { Text = "HO", IsCorrect = false }
                    }
                },
                new Question
                {
                    Text = "Which continent is Egypt in?",
                    Answers = new List<Answer>
                    {
                        new Answer { Text = "Africa", IsCorrect = true },
                        new Answer { Text = "Asia", IsCorrect = false },
                        new Answer { Text = "Europe", IsCorrect = false },
                        new Answer { Text = "South America", IsCorrect = false }
                    }
                },
                new Question
                {
                    Text = "What is the square root of 81?",
                    Answers = new List<Answer>
                    {
                        new Answer { Text = "8", IsCorrect = false },
                        new Answer { Text = "7", IsCorrect = false },
                        new Answer { Text = "9", IsCorrect = true },
                        new Answer { Text = "10", IsCorrect = false }
                    }
                },
                new Question
                {
                    Text = "Which language has the most native speakers?",
                    Answers = new List<Answer>
                    {
                        new Answer { Text = "Mandarin Chinese", IsCorrect = true },
                        new Answer { Text = "English", IsCorrect = false },
                        new Answer { Text = "Spanish", IsCorrect = false },
                        new Answer { Text = "Hindi", IsCorrect = false }
                    }
                }
            };

            db.Questions.AddRange(predefinedQuestions);
            await db.SaveChangesAsync();

            var random = new Random();
            var quizzes = new List<Quiz>();

            // 10 Quizzes using same pool of questions
            for (int q = 1; q <= 10; q++)
            {
                var quiz = new Quiz
                {
                    Title = $"General Knowledge Quiz {q}",
                    CreatedBy = admin,
                    Questions = predefinedQuestions
                        .OrderBy(_ => random.Next()) // Shuffle
                        .Take(5) // Take 5 random questions per quiz
                        .ToList()
                };
                quizzes.Add(quiz);
            }

            db.Quizzes.AddRange(quizzes);
            await db.SaveChangesAsync();
        }
    }
}
