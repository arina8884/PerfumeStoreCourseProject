namespace PerfumeStore.MVCC.ViewModels;

public class OrderQueueViewModel
{
    public IReadOnlyList<ManagerOrderSummaryViewModel> Orders { get; set; } =
        Array.Empty<ManagerOrderSummaryViewModel>();
}

public class ManagerOrderSummaryViewModel
{
    public int OrderId { get; set; }

    public DateTime OrderDate { get; set; }

    public string ClientFullName { get; set; } = string.Empty;

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = string.Empty;
}
