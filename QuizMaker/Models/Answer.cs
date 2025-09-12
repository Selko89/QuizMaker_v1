using System.ComponentModel.DataAnnotations;

namespace QuizMaker.Models
{
    public class Answer
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Answer text cannot be empty.")]
        [MinLength(1, ErrorMessage = "Answer must contain at least 1 character.")]
        public string Text { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }

        public int QuestionId { get; set; }

        public Question? Question { get; set; }

        
    }
}
