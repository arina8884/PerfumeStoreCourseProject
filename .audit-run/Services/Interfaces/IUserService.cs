using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Services.Interfaces;

public interface IUserService
{
    Task<bool> EmailExistsAsync(string email);

    Task<int> RegisterClientAsync(string fullName, string email, string password, string? phone);

    Task<ClientDashboardViewModel?> GetDashboardAsync(int userId);

    Task<ClientProfileViewModel?> GetProfileAsync(int userId);

    Task<EditProfileViewModel?> GetEditProfileAsync(int userId);

    Task<bool> UpdateProfileAsync(int userId, string fullName, string? phone);

    Task<UserListViewModel> GetAllUsersAsync();

    Task<IReadOnlyList<UserListItemViewModel>> GetRecentUsersAsync(int count);

    Task<UserDetailsViewModel?> GetUserDetailsAsync(int userId);

    Task<UserManageViewModel?> GetUserForManageAsync(int userId);

    Task<int> CreateUserAsync(UserManageViewModel model);

    Task<bool> UpdateUserAsync(UserManageViewModel model);
}
