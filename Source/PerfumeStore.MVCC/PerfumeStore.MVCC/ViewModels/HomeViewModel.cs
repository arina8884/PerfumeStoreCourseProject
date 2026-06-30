namespace PerfumeStore.MVCC.ViewModels;

public class HomeViewModel
{
    public IReadOnlyList<ProductCardViewModel> PopularProducts { get; set; } = [];

    public IReadOnlyList<ProductCardViewModel> NewProducts { get; set; } = [];

    public IReadOnlyList<CategoryViewModel> Categories { get; set; } = [];
}
