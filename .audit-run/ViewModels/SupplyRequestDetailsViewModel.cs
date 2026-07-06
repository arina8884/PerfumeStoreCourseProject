namespace PerfumeStore.MVCC.ViewModels;

public class SupplyRequestDetailsViewModel
{
    public int Id { get; set; }

    public string SupplierName { get; set; } = string.Empty;

    public string CreatedByFullName { get; set; } = string.Empty;

    public DateTime RequestDate { get; set; }

    public string Status { get; set; } = string.Empty;

    public string? Comment { get; set; }

    public IReadOnlyList<SupplyRequestItemViewModel> Items { get; set; } =
        Array.Empty<SupplyRequestItemViewModel>();

    public IReadOnlyList<string> AvailableStatuses { get; set; } =
        Array.Empty<string>();
}

public class SupplyRequestItemViewModel
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public int Quantity { get; set; }
}
