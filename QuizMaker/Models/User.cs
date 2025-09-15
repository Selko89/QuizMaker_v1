using System.ComponentModel.DataAnnotations;

namespace QuizMaker.Models
{
    public enum UserRole
    {
        SuperUser,
        Administrator
    }
    public class User
    {
        public int Id { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public UserRole? Role { get; set; }

    }
}
