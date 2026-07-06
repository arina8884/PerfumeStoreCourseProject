namespace PerfumeStore.MVCC.ViewModels;

public class OrderCancelViewModel
{
    public int OrderId { get; set; }

    public string Status { get; set; } = string.Empty;

    public string ClientFullName { get; set; } = string.Empty;

    public decimal TotalAmount { get; set; }
}
