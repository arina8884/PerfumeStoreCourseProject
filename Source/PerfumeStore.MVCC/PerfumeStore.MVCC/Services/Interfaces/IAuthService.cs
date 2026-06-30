using PerfumeStore.MVCC.Models;

namespace PerfumeStore.MVCC.Services.Interfaces;

public interface IAuthService
{
    Task<User?> ValidateUserAsync(string email, string password);
}
