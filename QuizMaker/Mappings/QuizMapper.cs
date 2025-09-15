using QuizMaker.DTOs;
using QuizMaker.Models;

namespace QuizMaker.Mappings
{
    public static class QuizMapper
    {
        public static SimpleQuizDto ToDtoSimple(Quiz q)
        {
            return new SimpleQuizDto
            {
                Id = q.Id,
                Title = q.Title,
                Description = q.Description,
            };
        }
        public static QuizDto ToDto(Quiz q)
        {
            return new QuizDto
            {
                Id = q.Id,
                Title = q.Title,
                Description = q.Description,
                CreatedById = q.CreatedById,
                Questions = q.Questions.Select(QuestionMapper.ToDto).ToList()
            };
        }
    }
}
