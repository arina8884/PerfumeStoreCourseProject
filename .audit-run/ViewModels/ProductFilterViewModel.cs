namespace PerfumeStore.MVCC.ViewModels;

public class ProductFilterViewModel
{
    public string? SearchQuery { get; set; }

    public int? CategoryId { get; set; }

    public string? Brand { get; set; }

    public int? Volume { get; set; }

    public decimal? MinPrice { get; set; }

    public decimal? MaxPrice { get; set; }

    public string? StockFilter { get; set; }

    public string? SortBy { get; set; }
}
