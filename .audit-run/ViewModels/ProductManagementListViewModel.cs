namespace PerfumeStore.MVCC.ViewModels;

public class ProductManagementListViewModel
{
    public IReadOnlyList<ProductManagementItemViewModel> Products { get; set; } =
        Array.Empty<ProductManagementItemViewModel>();
}

public class ProductManagementItemViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;

    public string CategoryName { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public bool IsActive { get; set; }

    public string? ImageUrl { get; set; }
}
