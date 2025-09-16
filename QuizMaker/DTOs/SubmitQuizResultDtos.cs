namespace QuizMaker.DTOs
{
    public class SubmitQuizResultDto
    {
        public int QuizId { get; set; }
        public List<int> SelectedAnswerIds { get; set; }
    }
}
