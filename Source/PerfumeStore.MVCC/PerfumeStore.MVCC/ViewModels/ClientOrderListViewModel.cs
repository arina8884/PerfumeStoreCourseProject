namespace PerfumeStore.MVCC.ViewModels;

public class ClientOrderListViewModel
{
    public IReadOnlyList<ClientOrderSummaryViewModel> Orders { get; set; } =
        Array.Empty<ClientOrderSummaryViewModel>();
}

public class ClientOrderSummaryViewModel
{
    public int OrderId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = string.Empty;
}
