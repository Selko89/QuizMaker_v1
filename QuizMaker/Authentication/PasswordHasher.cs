using System.Security.Cryptography;
using System.Text;

namespace QuizMaker.Authentication
{
    public static class PasswordHasher
    {
        // Simple SHA256 hash — fine for prototypes, replace with a stronger PBKDF2/BCrypt/Argon2 in production
        public static string GetHash(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public static bool Verify(string password, string hash)
        {
            return GetHash(password) == hash;
        }
    }
}
