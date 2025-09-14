using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizMaker.DTOs;
using QuizMaker.Services;
using System.Security.Claims;

namespace QuizMaker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        private readonly QuizService _quizService;

        public QuizController(QuizService quizService)
        {
            _quizService = quizService;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuizById(int id)
        {
            var quiz = await _quizService.GetQuizAsync(id);
            return Ok(quiz);
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllQuizzes([FromQuery] bool skipDeleted = false)
        {
            return Ok(await _quizService.GetAllQuizzesAsync(skipDeleted));
        }

        [AllowAnonymous]
        [HttpGet("fullDetails")]
        public async Task<IActionResult> GetAllQuizzesFullDetails([FromQuery] bool skipDeleted = false)
        {
            return Ok(await _quizService.GetAllQuizzesFullDetailsAsync(skipDeleted));
        }

        [AllowAnonymous]
        [HttpGet("deleted")]
        public async Task<IActionResult> GetDeletedQuizzes()
        {
            return Ok(await _quizService.GetDeletedQuizzesFullDetailsAsync());
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizDto dto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var quiz = await _quizService.CreateQuizAsync(dto, userId.Value);
            return Ok(quiz);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuiz(int id, [FromBody] UpdateQuizDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var quiz = await _quizService.UpdateQuizAsync(id, dto, userId.Value);
            return Ok(quiz);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            await _quizService.DeleteQuizAsync(id, userId.Value);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}/forever")]
        public async Task<IActionResult> DeleteQuizForever(int id)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            await _quizService.DeleteQuizForeverAsync(id, userId.Value);
            return NoContent();
        }

        private int? GetUserId()
        {
            var sub = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            if (int.TryParse(sub, out var id)) return id;
            return null;
        }
    }
}
