namespace PerfumeStore.MVCC.Models;

public class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public string Role { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public Cart? Cart { get; set; }

    public ICollection<Order> Orders { get; set; } = new List<Order>();

    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public ICollection<Review> Reviews { get; set; } = new List<Review>();

    public ICollection<SupplyRequest> SupplyRequests { get; set; } = new List<SupplyRequest>();
}
