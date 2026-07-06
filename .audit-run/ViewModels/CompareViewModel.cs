namespace PerfumeStore.MVCC.ViewModels;

public class CompareViewModel
{
    public IReadOnlyList<ProductCompareItemViewModel> Products { get; set; } =
        Array.Empty<ProductCompareItemViewModel>();
}

public class ProductCompareItemViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;

    public string CategoryName { get; set; } = string.Empty;

    public int Volume { get; set; }

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public double? AverageRating { get; set; }

    public int ReviewsCount { get; set; }

    public string StockStatusLabel { get; set; } = string.Empty;
}
