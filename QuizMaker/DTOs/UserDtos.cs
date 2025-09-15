using QuizMaker.Models;

namespace QuizMaker.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string NickName { get; set; } = string.Empty;
    }

    public class UpdateUserDto
    {
        public string? Email { get; set; }
        public string? NickName { get; set; }
        public UserRole? Role { get; set; }
        public string? Password { get; set; }
    }
}
