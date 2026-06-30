namespace PerfumeStore.MVCC.ViewModels;

public class ProductCatalogViewModel
{
    public IReadOnlyList<ProductCardViewModel> Products { get; set; } = [];

    public IReadOnlyList<CategoryViewModel> Categories { get; set; } = [];

    public ProductFilterViewModel Filter { get; set; } = new();
}
