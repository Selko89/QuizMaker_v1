using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizMaker.DTOs;
using QuizMaker.Helpers;
using QuizMaker.Models;
using QuizMaker.Services;

namespace QuizMaker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator,SuperUser")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto dto)
        {
            var userId = User.GetUserId();
            if (userId == null) return Unauthorized();

            var currentUserRole = User.GetUserRole();

            var updatedUser = await _userService.UpdateUserAsync(id, dto, (currentUserRole == UserRole.Administrator || currentUserRole == UserRole.SuperUser));
            if (updatedUser == null) return NotFound();
            return Ok(updatedUser);
        }
    }
}
