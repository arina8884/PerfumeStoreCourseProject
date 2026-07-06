namespace PerfumeStore.MVCC.ViewModels;

public class SupplyRequestListViewModel
{
    public IReadOnlyList<SupplyRequestSummaryViewModel> Requests { get; set; } =
        Array.Empty<SupplyRequestSummaryViewModel>();
}

public class SupplyRequestSummaryViewModel
{
    public int Id { get; set; }

    public string SupplierName { get; set; } = string.Empty;

    public DateTime RequestDate { get; set; }

    public string Status { get; set; } = string.Empty;

    public string CreatedByFullName { get; set; } = string.Empty;
}
