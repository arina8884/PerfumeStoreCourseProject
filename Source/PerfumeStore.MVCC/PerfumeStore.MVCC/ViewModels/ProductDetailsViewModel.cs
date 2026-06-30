namespace PerfumeStore.MVCC.ViewModels;

public class ProductDetailsViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int Volume { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public int StockQuantity { get; set; }

    public string? ImageUrl { get; set; }
}
