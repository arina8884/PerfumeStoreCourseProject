namespace PerfumeStore.MVCC.ViewModels;

public class ClientProfileViewModel
{
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public string Role { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
