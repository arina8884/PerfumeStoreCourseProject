namespace PerfumeStore.MVCC.ViewModels;

public class OrderTrackingViewModel
{
    public int OrderId { get; set; }

    public DateTime OrderDate { get; set; }

    public string Status { get; set; } = string.Empty;
}
