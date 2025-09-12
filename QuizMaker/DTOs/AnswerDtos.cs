using System.ComponentModel.DataAnnotations;

namespace QuizMaker.DTOs
{
    public class AnswerDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }

    public class CreateAnswerDto
    {
        [Required, MinLength(1)]
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }

        [Required]
        public int QuestionId { get; set; }
    }

    public class UpdateAnswerDto
    {
        [Required, MinLength(1)]
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }
}
