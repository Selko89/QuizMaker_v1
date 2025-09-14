using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizMaker.DTOs;
using QuizMaker.Models;
using QuizMaker.Services;

namespace QuizMaker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly QuestionService _questionService;

        public QuestionController(QuestionService questionService)
        {
            _questionService = questionService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllQuestions()
        {
            return Ok(await _questionService.GetAllQuestionsAsync());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var question = await _questionService.GetQuestionAsync(id);
            return Ok(question);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateQuestion([FromBody] CreateQuestionDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _questionService.CreateQuestionAsync(dto);
            return Ok(created);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestion(int id, [FromBody] UpdateQuestionDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updated = await _questionService.UpdateQuestionAsync(id, dto);
            return Ok(updated);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            await _questionService.DeleteQuestionAsync(id);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("search")]
        public async Task<IActionResult> SearchQuestions([FromQuery] string? search, int page = 1, int pageSize = 20)
        {
            var questions = await _questionService.SearchQuestionsAsync(search, page, pageSize);
            return Ok(questions);
        }

    }
}
