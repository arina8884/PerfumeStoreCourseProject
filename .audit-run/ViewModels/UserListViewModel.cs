namespace PerfumeStore.MVCC.ViewModels;

public class UserListViewModel
{
    public IReadOnlyList<UserListItemViewModel> Users { get; set; } =
        Array.Empty<UserListItemViewModel>();
}

public class UserListItemViewModel
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
