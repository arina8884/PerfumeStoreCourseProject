namespace PerfumeStore.MVCC.ViewModels;

public class ContentManagerDashboardViewModel
{
    public int ProductsCount { get; set; }

    public int ActiveProductsCount { get; set; }

    public int CategoriesCount { get; set; }

    public int ArchivedProductsCount { get; set; }

    public IReadOnlyList<ProductCardViewModel> RecentProducts { get; set; } =
        Array.Empty<ProductCardViewModel>();

    public IReadOnlyList<CategoryViewModel> RecentCategories { get; set; } =
        Array.Empty<CategoryViewModel>();
}
