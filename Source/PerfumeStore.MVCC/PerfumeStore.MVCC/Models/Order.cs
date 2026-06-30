namespace PerfumeStore.MVCC.Models;

public class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public decimal TotalAmount { get; set; }

    public string DeliveryAddress { get; set; } = string.Empty;

    public string PaymentMethod { get; set; } = string.Empty;

    public string DeliveryMethod { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public User User { get; set; } = null!;

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
