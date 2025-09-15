using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizMaker.Data;
using QuizMaker.DTOs;
using QuizMaker.Mappings;
using QuizMaker.Models;

namespace QuizMaker.Services
{
    public class QuizService
    {
        private readonly AppDbContext _db;

        public QuizService(AppDbContext db) {
            _db = db;
        }

        public async Task<QuizDto> GetQuizAsync(int id)
        {
            var quiz =  await _db.Quizzes.Include(q => q.Questions).ThenInclude(q => q.Answers).FirstOrDefaultAsync(q => q.Id == id);

            if (quiz == null)
            {
                throw new Exception("Quiz not found");
            }

            return QuizMapper.ToDto(quiz);
        }

        public async Task<List<SimpleQuizDto>> GetAllQuizzesAsync(bool skipDeleted)
        {
            var query = _db.Quizzes.AsQueryable();

            if (skipDeleted)
            {
                query = query.Where(q => q.DeletedOn == null);     
            }

            var quizzes = await query.ToListAsync();
            return quizzes.Select(QuizMapper.ToDtoSimple).ToList();
        }

        public async Task<List<QuizDto>> GetAllQuizzesFullDetailsAsync(bool skipDeleted)
        {
            var query = _db.Quizzes.Include(q => q.Questions).ThenInclude(q => q.Answers).AsQueryable();

            if (skipDeleted)
            {
                query = query.Where(q => q.DeletedOn == null);
            }

            var quizzes = await query.ToListAsync();
            return quizzes.Select(QuizMapper.ToDto).ToList();
        }

        public async Task<List<QuizDto>> GetDeletedQuizzesFullDetailsAsync()
        {
            var quizzes = await _db.Quizzes.Where(q => q.DeletedOn != null).ToListAsync();

            return quizzes.Select(QuizMapper.ToDto).ToList();
        }

        public async Task<QuizDto> CreateQuizAsync(CreateQuizDto dto, int userId)
        {
            var questions = await _db.Questions.Where(q => dto.QuestionIds.Contains(q.Id)).ToListAsync();

            if (questions == null || questions.Count == 0)
            {
                throw new Exception("Quiz must contain minimum one question");
            }

            var quiz = new Quiz { Title = dto.Title, Description = dto.Description, Questions = questions, CreatedById = userId };

            _db.Quizzes.Add(quiz);
            await _db.SaveChangesAsync();

            return await GetQuizAsync(quiz.Id);
        }

        public async Task<QuizDto> UpdateQuizAsync(int id, UpdateQuizDto dto, int userId, bool canUpdate)
        {
            var quiz = await _db.Quizzes.Include(q => q.Questions).FirstOrDefaultAsync(q => q.Id == id);

            if (quiz == null)
            {
                throw new Exception("Quiz not found");
            }

            if (quiz.CreatedById != userId && !canUpdate) throw new UnauthorizedAccessException();

            quiz.Title = dto.Title;
            quiz.Description = dto.Description;

            if (dto.QuestionIds != null && dto.QuestionIds.Any())
            {
                var questions = await _db.Questions.Where(q => dto.QuestionIds.Contains(q.Id)).ToListAsync();

                if (questions == null || questions.Count == 0)
                {
                    throw new Exception("Quiz must contain minimum one question");
                }

                quiz.Questions = questions;
            }
            else
            {
                throw new Exception("Quiz must contain minimum one question");
            }

            await _db.SaveChangesAsync();

            return await GetQuizAsync(id);
        }

        public async Task<bool> DeleteQuizAsync(int id, int userId, bool canDelete)
        {
            var quiz = await _db.Quizzes.FindAsync(id);
            if (quiz == null)
            {
                throw new Exception("Quiz not found");
            }
            if (quiz.CreatedById != userId && !canDelete) throw new UnauthorizedAccessException();

            quiz.DeletedOn = DateTime.Now;

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteQuizForeverAsync(int id, int userId, bool canDelete)
        {
            var quiz = await _db.Quizzes.FindAsync(id);
            if (quiz == null)
            {
                throw new Exception("Quiz not found");
            }

            if (quiz.CreatedById != userId && !canDelete) throw new UnauthorizedAccessException();

            _db.Quizzes.Remove(quiz);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
