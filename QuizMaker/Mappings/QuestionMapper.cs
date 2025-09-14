using QuizMaker.DTOs;
using QuizMaker.Models;

namespace QuizMaker.Mappings
{
    public static class QuestionMapper
    {
        public static QuestionDto ToDto(Question q)
        {
            return new QuestionDto
            {
                Id = q.Id,
                Text = q.Text,
                Answers = q.Answers.Select(AnswerMapper.ToDto).ToList()
            };
        }

        public static Question FromCreateDto(CreateQuestionDto dto)
        {
            return new Question
            {
                Text = dto.Text,
                Answers = dto.Answers?.Select(a => AnswerMapper.FromCreateDto(a)).ToList() ?? new List<Answer>()
            };
        }

        public static void MapUpdate(Question q, UpdateQuestionDto dto)
        {
            q.Text = dto.Text;
        }

        public static List<QuestionDto> ToDtoList(IEnumerable<Question> questions)
        {
            return questions.Select(ToDto).ToList();
        }
    }
}
