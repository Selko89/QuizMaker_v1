using System.ComponentModel.DataAnnotations;

namespace QuizMaker.Models
{
    public class Quiz
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Quiz title cannot be empty.")]
        [MinLength(1, ErrorMessage = "Quiz must contain at least 1 character.")]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime OnChange { get; set; } = DateTime.Now;
        public DateTime? DeletedOn { get; set; }

        public ICollection<Question> Questions { get; set; } = new List<Question>();

        public int? CreatedById { get; set; }
        public User? CreatedBy { get; set; }

    }
}
