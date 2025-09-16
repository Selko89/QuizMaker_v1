using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizMaker.DTOs;
using QuizMaker.Helpers;
using QuizMaker.Mappings;
using QuizMaker.Models;
using QuizMaker.Services;

namespace QuizMaker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizResultController : ControllerBase
    {
        private readonly QuizResultService _quizResultService;

        public QuizResultController(QuizResultService quizResultService)
        {
            _quizResultService = quizResultService;
        }

        // Submit quiz result
        [Authorize]
        [HttpPost("submit")]
        public async Task<IActionResult> Submit([FromBody] SubmitQuizResultDto dto)
        {
            var userId = User.GetUserId();
            if (userId == null) return Unauthorized();

            var result = await _quizResultService.SubmitQuizResultAsync(userId.Value, dto.QuizId, dto.SelectedAnswerIds);

            return Ok(result);
        }

        // Get results by user
        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var results = await _quizResultService.GetResultsByUserAsync(userId);

            return Ok(results);
        }

        // Get results by quiz
        [Authorize]
        [HttpGet("quiz/{quizId}")]
        public async Task<IActionResult> GetByQuiz(int quizId)
        {
            var results = await _quizResultService.GetResultsByQuizAsync(quizId);
            return Ok(results);
        }
    }
}

