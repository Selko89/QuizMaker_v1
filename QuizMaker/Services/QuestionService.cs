using Microsoft.EntityFrameworkCore;
using QuizMaker.Data;
using QuizMaker.DTOs;
using QuizMaker.Mappings;
using QuizMaker.Models;

namespace QuizMaker.Services
{
    public class QuestionService
    {
        private readonly AppDbContext _db;

        public QuestionService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<QuestionDto> GetQuestionAsync(int id)
        {
            var question = await _db.Questions.Include(q => q.Answers).FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
            {
                throw new Exception("Question not found");
            }
            return QuestionMapper.ToDto(question);
        }

        public async Task<List<QuestionDto>> GetAllQuestionsAsync()
        {
            var questions = await _db.Questions.Include(q => q.Answers).ToListAsync();
            return questions.Select(QuestionMapper.ToDto).ToList();
        }

        public async Task<QuestionDto> CreateQuestionAsync(CreateQuestionDto dto)
        {
            var question = new Question
            {
                Text = dto.Text,
                Answers = dto.Answers?.Select(a => new Answer { Text = a.Text, IsCorrect = a.IsCorrect }).ToList() ?? new List<Answer>()
            };

            _db.Questions.Add(question);
            await _db.SaveChangesAsync();
            return await GetQuestionAsync(question.Id);
        }

        public async Task<QuestionDto> UpdateQuestionAsync(int id, UpdateQuestionDto dto)
        {
            var question = await _db.Questions.Include(x => x.Answers).FirstOrDefaultAsync(x => x.Id == id);
            if (question == null)
            {
                throw new Exception("Question not found");
            }

            question.Text = dto.Text;

            if (dto.Answers != null && dto.Answers.Any())
            {
                _db.Answers.RemoveRange(question.Answers);
                question.Answers = dto.Answers.Select(a => new Answer { Text = a.Text, IsCorrect = a.IsCorrect }).ToList();
            }

            await _db.SaveChangesAsync();
            return await GetQuestionAsync(id);
        }

        public async Task<bool> DeleteQuestionAsync(int id)
        {
            var question = await _db.Questions.FindAsync(id);
            if (question == null)
            {
                throw new Exception("Question not found");
            }

            _db.Questions.Remove(question);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<QuestionDto>> SearchQuestionsAsync(string? search, int page = 1, int pageSize = 20)
        {
            var query = _db.Questions
                .Include(q => q.Answers)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(q => q.Text.Contains(search));
            }

            var questions = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return QuestionMapper.ToDtoList(questions);
        }

    }
}
