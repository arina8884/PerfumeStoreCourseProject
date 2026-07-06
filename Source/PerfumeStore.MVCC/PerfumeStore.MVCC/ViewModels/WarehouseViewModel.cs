namespace PerfumeStore.MVCC.ViewModels;

public class WarehouseViewModel
{
    public int TotalProducts { get; set; }

    public int ActiveProducts { get; set; }

    public int LowStockCount { get; set; }

    public int OutOfStockCount { get; set; }

    public IReadOnlyList<WarehouseItemViewModel> Items { get; set; } =
        Array.Empty<WarehouseItemViewModel>();
}

public class WarehouseItemViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;

    public string CategoryName { get; set; } = string.Empty;

    public int StockQuantity { get; set; }

    public bool IsActive { get; set; }

    public string? ImageUrl { get; set; }
}
