namespace PerfumeStore.MVCC.ViewModels;

public class ProductPreviewViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;

    public string CategoryName { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Volume { get; set; }

    public string? Description { get; set; }

    public int StockQuantity { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; }
}
