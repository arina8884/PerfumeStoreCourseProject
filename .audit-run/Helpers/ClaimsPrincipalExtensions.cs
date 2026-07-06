using System.Security.Claims;

namespace PerfumeStore.MVCC.Helpers;

public static class ClaimsPrincipalExtensions
{
    public static bool TryGetUserId(this ClaimsPrincipal principal, out int userId)
    {
        userId = 0;
        var value = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        return int.TryParse(value, out userId);
    }

    public static int GetRequiredUserId(this ClaimsPrincipal principal)
    {
        if (principal.TryGetUserId(out var userId))
        {
            return userId;
        }

        throw new UnauthorizedAccessException("Не удалось определить пользователя.");
    }
}
