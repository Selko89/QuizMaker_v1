using QuizMaker.DTOs;
using QuizMaker.Models;

namespace QuizMaker.Mappings
{
    public static class UserMapper
    {
        public static UserDto ToDto(User u)
        {
            return new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                NickName = u.NickName
            };
        }
    }
}
