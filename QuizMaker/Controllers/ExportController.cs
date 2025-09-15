using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizMaker.Services;

namespace QuizMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private readonly QuizService _quizService;
        private readonly QuizExportService _exportService;

        public ExportController(QuizService quizService, QuizExportService exportService)
        {
            _quizService = quizService;
            _exportService = exportService;
        }

        [HttpGet("available")]
        public IActionResult GetAvailable()
        {
            return Ok(_exportService.GetAvailableExporters());
        }

        [HttpGet("{id}/export/{format}")]
        public async Task<IActionResult> ExportQuiz(int id, string format)
        {
            var quiz = await _quizService.GetQuizAsync(id);
            if (quiz == null) return NotFound();

            var result = _exportService.Export(format, quiz);
            if (result == null) return BadRequest(new { error = "Exporter not found." });

            var fileNameSafe = string.Concat(quiz.Title.Split(Path.GetInvalidFileNameChars())).Trim();
            return File(result.Value.Content, result.Value.ContentType, $"{fileNameSafe}.{format}");
        }
    }
}
