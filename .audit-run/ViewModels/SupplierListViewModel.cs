namespace PerfumeStore.MVCC.ViewModels;

public class SupplierListViewModel
{
    public IReadOnlyList<SupplierItemViewModel> Suppliers { get; set; } =
        Array.Empty<SupplierItemViewModel>();
}

public class SupplierItemViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public int SupplyRequestsCount { get; set; }
}
