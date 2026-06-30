namespace PerfumeStore.MVCC.ViewModels;

public class OrderResultViewModel
{
    public int OrderId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string DeliveryAddress { get; set; } = string.Empty;

    public string DeliveryMethod { get; set; } = string.Empty;

    public string PaymentMethod { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;
}
