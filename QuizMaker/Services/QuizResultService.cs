using Microsoft.EntityFrameworkCore;
using QuizMaker.Data;
using QuizMaker.DTOs;
using QuizMaker.Mappings;
using QuizMaker.Models;

namespace QuizMaker.Services
{
    public class QuizResultService
    {
        private readonly AppDbContext _context;

        public QuizResultService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<QuizResultDto> SubmitQuizResultAsync(int userId, int quizId, List<int> selectedAnswerIds)
        {
            var quiz = await _context.Quizzes.Include(q => q.Questions).ThenInclude(q => q.Answers).FirstOrDefaultAsync(q => q.Id == quizId);

            if (quiz == null) {
                throw new Exception("Quiz not found");
            };

            int correctAnswers = 0;
            var quizResultAnswers = new List<QuizResultAnswer>();

            foreach (var question in quiz.Questions)
            {
                var selectedAnswer = question.Answers.FirstOrDefault(a => selectedAnswerIds.Contains(a.Id));
                bool isCorrect = selectedAnswer?.IsCorrect ?? false;
                if (isCorrect) {
                    correctAnswers++;
                }

                quizResultAnswers.Add(new QuizResultAnswer
                {
                    QuestionId = question.Id,
                    AnswerId = selectedAnswer?.Id ?? 0,
                    IsCorrect = isCorrect
                });
            }

            var quizResult = new QuizResult
            {
                UserId = userId,
                QuizId = quizId,
                TotalQuestions = quiz.Questions.Count,
                CorrectAnswers = correctAnswers,
                Percentage = (double)correctAnswers / quiz.Questions.Count * 100,
                FinishedAt = DateTime.Now,
                Answers = quizResultAnswers
            };

            _context.QuizResults.Add(quizResult);
            await _context.SaveChangesAsync();

            var dto = QuizResultMapper.ToDto(quizResult);
            return dto;
        }

        // Get results by user
        public async Task<List<QuizResultDto>> GetResultsByUserAsync(int userId)
        {
            var result =  await _context.QuizResults.Include(qr => qr.Answers).ThenInclude(a => a.Question).Include(qr => qr.Quiz).Where(qr => qr.UserId == userId).ToListAsync();

            if (result == null)
            {
                throw new Exception("Quiz result not found");
            }

            return result.Select(QuizResultMapper.ToDto).ToList();
        }

        // Get results by quiz
        public async Task<List<QuizResultDto>> GetResultsByQuizAsync(int quizId)
        {
            var result = await _context.QuizResults.Include(qr => qr.Answers).ThenInclude(a => a.Question).Include(qr => qr.Quiz).Where(qr => qr.QuizId == quizId).ToListAsync();

            if (result == null)
            {
                throw new Exception("Quiz result not found");
            }

            return result.Select(QuizResultMapper.ToDto).ToList();
        }
    }
}

