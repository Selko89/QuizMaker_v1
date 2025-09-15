using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizMaker.DTOs;
using QuizMaker.Helpers;
using QuizMaker.Services;

namespace QuizMaker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnswerController : ControllerBase
    {
        private readonly AnswerService _answerService;

        public AnswerController(AnswerService answerService)
        {
            _answerService = answerService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(new { message = "Use Questions endpoints." });
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(new { message = "Use Question endpoints" });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAnswer([FromBody] CreateAnswerDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = User.GetUserId();
            if (userId == null) return Unauthorized();

            var created = await _answerService.CreateAnswerAsync(dto);
            return Ok(created);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnswer(int id, [FromBody] UpdateAnswerDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = User.GetUserId();
            if (userId == null) return Unauthorized();

            var updated = await _answerService.UpdateAnswerAsync(id, dto);
            return Ok(updated);
        }

        [Authorize(Roles = "Administrator,SuperUser")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            await _answerService.DeleteAnswerAsync(id);
            return NoContent();
        }
    }
}
