namespace QuizMaker.Models
{
    public class QuizResult
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }
        public double Percentage { get; set; }

        public DateTime FinishedAt { get; set; } = DateTime.UtcNow;

        //List of answers selected
        public ICollection<QuizResultAnswer> Answers { get; set; } = new List<QuizResultAnswer>();
    }
}
