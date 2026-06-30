namespace PerfumeStore.MVCC.Services.Interfaces;

public interface IUserService
{
    Task<bool> EmailExistsAsync(string email);

    Task<int> RegisterClientAsync(string fullName, string email, string password, string? phone);
}
