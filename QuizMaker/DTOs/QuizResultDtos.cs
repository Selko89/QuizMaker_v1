namespace QuizMaker.DTOs
{
    public class QuizResultDto
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public int UserId { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public double Percentage { get; set; }
        public DateTime FinishedAt { get; set; }
        public List<AnswerQuestionDto> Answers { get; set; }
    }

    public class AnswerQuestionDto
    {
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
    }

}
