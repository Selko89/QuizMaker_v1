using System.ComponentModel.DataAnnotations;

namespace QuizMaker.DTOs
{
    public class QuizDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<QuestionDto> Questions { get; set; } = new();
        public int? CreatedById { get; set; }
    }

    public class CreateQuizDto
    {
        [Required, MinLength(1)]
        public string Title { get; set; } = string.Empty;
        public List<int> QuestionIds { get; set; } = new();
    }

    public class UpdateQuizDto
    {
        [Required, MinLength(1)]
        public string Title { get; set; } = string.Empty;
        public List<int> QuestionIds { get; set; } = new();
    }
}
