using Microsoft.AspNetCore.Mvc;
using QuizMaker.Data;
using QuizMaker.DTOs;
using QuizMaker.Services;

namespace QuizMaker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _auth;
        public AuthController(AuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var user = await _auth.RegisterAsync(dto);
                return Ok(new { user.Id, user.Email, user.NickName });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var token = await _auth.LoginAsync(dto);
            if (token == null) return Unauthorized(new { error = "Invalid credentials" });

            // get user to include nickname and id
            var user = await _auth.GetUserInfoAsync(dto.Email);
            return Ok(new AuthResponseDto { Token = token, UserId = user.Id, NickName = user.NickName });
        }

    }
}
