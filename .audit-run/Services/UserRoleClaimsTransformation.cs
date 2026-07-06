using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using PerfumeStore.MVCC.Data;
using PerfumeStore.MVCC.Helpers;

namespace PerfumeStore.MVCC.Services;

public class UserRoleClaimsTransformation : IClaimsTransformation
{
    private readonly PerfumeStoreDbContext _context;

    public UserRoleClaimsTransformation(PerfumeStoreDbContext context)
    {
        _context = context;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.Identity?.IsAuthenticated != true)
        {
            return principal;
        }

        if (!principal.TryGetUserId(out var userId))
        {
            return principal;
        }

        var user = await _context.Users
            .AsNoTracking()
            .Where(item => item.Id == userId)
            .Select(item => new { item.Role, item.FullName })
            .FirstOrDefaultAsync();

        if (user is null)
        {
            return principal;
        }

        if (principal.Identity is not ClaimsIdentity identity)
        {
            return principal;
        }

        ReplaceClaim(identity, ClaimTypes.Role, user.Role);
        ReplaceClaim(identity, ClaimTypes.Name, user.FullName);

        return principal;
    }

    private static void ReplaceClaim(ClaimsIdentity identity, string claimType, string value)
    {
        var existingClaims = identity.FindAll(claimType).ToList();

        foreach (var claim in existingClaims)
        {
            identity.RemoveClaim(claim);
        }

        identity.AddClaim(new Claim(claimType, value));
    }
}
