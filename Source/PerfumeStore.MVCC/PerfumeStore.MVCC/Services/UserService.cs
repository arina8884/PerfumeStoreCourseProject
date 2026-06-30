using Microsoft.EntityFrameworkCore;
using PerfumeStore.MVCC.Constants;
using PerfumeStore.MVCC.Data;
using PerfumeStore.MVCC.Models;
using PerfumeStore.MVCC.Services.Interfaces;

namespace PerfumeStore.MVCC.Services;

public class UserService : IUserService
{
    private readonly PerfumeStoreDbContext _context;

    public UserService(PerfumeStoreDbContext context)
    {
        _context = context;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        var normalizedEmail = email.Trim();

        return await _context.Users
            .AsNoTracking()
            .AnyAsync(user => user.Email == normalizedEmail);
    }

    public async Task<int> RegisterClientAsync(
        string fullName,
        string email,
        string password,
        string? phone)
    {
        var user = new User
        {
            FullName = fullName.Trim(),
            Email = email.Trim(),
            Phone = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim(),
            PasswordHash = PasswordHashHelper.ComputeSha256(password),
            Role = UserRoles.Client
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user.Id;
    }
}
