using Microsoft.EntityFrameworkCore;
using QuizMaker.Data;
using QuizMaker.DTOs;
using QuizMaker.Mappings;
using QuizMaker.Models;

namespace QuizMaker.Services
{
    public class AnswerService
    {
        private readonly AppDbContext _db;

        public AnswerService(AppDbContext db)
        {
            _db = db;
        }
        public async Task<AnswerDto> GetAnswerByIdAsync(int id)
        {
            var answer = await _db.Answers
                .Include(a => a.Question)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (answer == null)
            {
                throw new Exception("Answer not found");
            }
            return AnswerMapper.ToDto(answer);
        }

        public async Task<List<Answer>> GetAllAnswersAsync()
        {
            return await _db.Answers
                .Include(a => a.Question)
                .ToListAsync();
        }

        public async Task<AnswerDto> CreateAnswerAsync(CreateAnswerDto dto)
        {
            var question = await _db.Questions.FindAsync(dto.QuestionId) ?? throw new ArgumentException("Question not found");
            var answer = new Answer { Text = dto.Text, IsCorrect = dto.IsCorrect, QuestionId = dto.QuestionId };
            _db.Answers.Add(answer);
            await _db.SaveChangesAsync();
            return await GetAnswerByIdAsync(answer.Id);
        }

        public async Task<AnswerDto> UpdateAnswerAsync(int id, UpdateAnswerDto dto)
        {
            var answer = await _db.Answers.FindAsync(id) ?? throw new ArgumentException("Answer not found");
            answer.Text = dto.Text;
            answer.IsCorrect = dto.IsCorrect;
            await _db.SaveChangesAsync();
            return await GetAnswerByIdAsync(id);
        }

        public async Task<bool> DeleteAnswerAsync(int id)
        {
            var answer = await _db.Answers.FindAsync(id) ?? throw new ArgumentException("Answer not found");

            _db.Answers.Remove(answer);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
