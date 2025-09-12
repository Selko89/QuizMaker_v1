using QuizMaker.DTOs;
using QuizMaker.Models;

namespace QuizMaker.Mappings
{
    public static class AnswerMapper
    {
        public static AnswerDto ToDto(Answer a)
        {
            return new AnswerDto
            {
                Id = a.Id,
                Text = a.Text,
                IsCorrect = a.IsCorrect
            };
        }

        public static Answer FromCreateDto(CreateAnswerDto dto)
        {
            return new Answer {
                Text = dto.Text,
                IsCorrect = dto.IsCorrect,
                QuestionId = dto.QuestionId
            };
        }

        public static void MapUpdate(Answer a, UpdateAnswerDto dto)
        {
            a.Text = dto.Text;
            a.IsCorrect = dto.IsCorrect;
        }
    }
}
