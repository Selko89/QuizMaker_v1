using QuizMaker.DTOs;
using QuizMaker.Models;

namespace QuizMaker.Mappings
{
    public static class QuizResultMapper
    {
        public static QuizResultDto ToDto(QuizResult quizResult)
        {
            if (quizResult == null) return null;

            return new QuizResultDto
            {
                Id = quizResult.Id,
                QuizId = quizResult.QuizId,
                UserId = quizResult.UserId,
                TotalQuestions = quizResult.TotalQuestions,
                CorrectAnswers = quizResult.CorrectAnswers,
                Percentage = quizResult.Percentage,
                FinishedAt = quizResult.FinishedAt,
                Answers = quizResult.Answers?
                    .Select(a => new AnswerQuestionDto
                    {
                        QuestionId = a.QuestionId,
                        AnswerId = a.AnswerId
                    })
                    .ToList() ?? new List<AnswerQuestionDto>()
            };
        }
    }

}
