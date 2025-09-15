using QuizMaker.Models;
using System.Security.Claims;

namespace QuizMaker.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static int? GetUserId(this ClaimsPrincipal user)
        {
            var sub = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue("sub");
            if (int.TryParse(sub, out var id)) return id;
            return null;
        }

        public static UserRole? GetUserRole(this ClaimsPrincipal user)
        {
            if (user?.Identity?.IsAuthenticated != true) return null;
            var roleClaim = user.FindFirst(ClaimTypes.Role);
            return roleClaim != null ? Enum.Parse<UserRole>(roleClaim.Value) : null;
        }
    }
}
