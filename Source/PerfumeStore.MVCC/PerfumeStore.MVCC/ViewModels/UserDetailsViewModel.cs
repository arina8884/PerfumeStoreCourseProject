namespace PerfumeStore.MVCC.ViewModels;

public class UserDetailsViewModel
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public string Role { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public int OrdersCount { get; set; }
}
