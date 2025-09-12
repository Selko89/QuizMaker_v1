using System.ComponentModel.DataAnnotations;

namespace QuizMaker.Models
{
    public class Question
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Question text cannot be empty.")]
        [MinLength(1, ErrorMessage = "Question must contain at least 1 character.")]
        public string Text { get; set; } = string.Empty;

        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
        public ICollection<Answer> Answers { get; set; } = new List<Answer>();

    }
}
