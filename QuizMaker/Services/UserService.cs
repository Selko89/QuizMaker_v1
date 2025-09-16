using Microsoft.EntityFrameworkCore;
using QuizMaker.Authentication;
using QuizMaker.Data;
using QuizMaker.DTOs;
using QuizMaker.Mappings;
using QuizMaker.Models;

namespace QuizMaker.Services
{
    public class UserService
    {
        private readonly AppDbContext _db;

        public UserService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _db.Users.ToListAsync();
            return users.Select(UserMapper.ToDto).ToList();
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            return UserMapper.ToDto(user);
        }

        public async Task<UserDto?> UpdateUserAsync(int currentUserId, int userId, UpdateUserDto dto, bool isAdmin)
        {
            var user = await _db.Users.FindAsync(userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            if (isAdmin || (currentUserId == user.Id))
            {
                if (!string.IsNullOrEmpty(dto.Email))
                    user.Email = dto.Email;

                if (!string.IsNullOrEmpty(dto.NickName))
                    user.NickName = dto.NickName;
            }
            else
            {
                throw new Exception("You are not autorized to change this user");
            }

            // Update user (admin only)
            if (isAdmin)
            {
                if (dto.Role.HasValue)
                    user.Role = dto.Role.Value;

                if (!string.IsNullOrEmpty(dto.Password))
                    user.PasswordHash = PasswordHasher.GetHash(dto.Password);
            }

            await _db.SaveChangesAsync();
            return UserMapper.ToDto(user);
        }
    }
}
