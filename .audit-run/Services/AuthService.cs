using Microsoft.EntityFrameworkCore;
using PerfumeStore.MVCC.Data;
using PerfumeStore.MVCC.Models;
using PerfumeStore.MVCC.Services.Interfaces;

namespace PerfumeStore.MVCC.Services;

public class AuthService : IAuthService
{
    private readonly PerfumeStoreDbContext _context;

    public AuthService(PerfumeStoreDbContext context)
    {
        _context = context;
    }

    public async Task<User?> ValidateUserAsync(string email, string password)
    {
        var normalizedEmail = email.Trim();

        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(currentUser => currentUser.Email == normalizedEmail);

        if (user is null || !IsPasswordValid(user.PasswordHash, password))
        {
            return null;
        }

        return user;
    }

    private static bool IsPasswordValid(string storedPasswordHash, string password)
    {
        return storedPasswordHash == password ||
            storedPasswordHash == PasswordHashHelper.ComputeSha256(password);
    }
}
