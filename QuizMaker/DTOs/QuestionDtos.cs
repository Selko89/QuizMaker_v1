using System.ComponentModel.DataAnnotations;

namespace QuizMaker.DTOs
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public List<AnswerDto> Answers { get; set; } = new();
    }

    public class CreateQuestionDto
    {
        [Required, MinLength(1)]
        public string Text { get; set; } = string.Empty;

        // optionally create answers inline
        public List<CreateAnswerDto> Answers { get; set; } = new();
    }

    public class UpdateQuestionDto
    {
        [Required, MinLength(1)]
        public string Text { get; set; } = string.Empty;
        public List<UpdateAnswerDto> Answers { get; set; } = new();
    }
}
