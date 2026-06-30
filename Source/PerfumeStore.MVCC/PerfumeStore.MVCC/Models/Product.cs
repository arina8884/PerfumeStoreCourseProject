namespace PerfumeStore.MVCC.Models;

public class Product
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Volume { get; set; }

    public string? Description { get; set; }

    public int StockQuantity { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public Category Category { get; set; } = null!;

    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public ICollection<Review> Reviews { get; set; } = new List<Review>();

    public ICollection<SupplyRequestItem> SupplyRequestItems { get; set; } = new List<SupplyRequestItem>();
}
