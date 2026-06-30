namespace PerfumeStore.MVCC.ViewModels;

public class ProductFilterViewModel
{
    public string? SearchQuery { get; set; }

    public int? CategoryId { get; set; }

    public string? SortBy { get; set; }
}
