namespace PerfumeStore.MVCC.ViewModels;

public class HomeViewModel
{
    public IReadOnlyList<ProductCardViewModel> PopularProducts { get; set; } = [];

    public IReadOnlyList<ProductCardViewModel> NewProducts { get; set; } = [];

    public IReadOnlyList<CategoryViewModel> Categories { get; set; } = [];

    public IReadOnlyList<string> Brands { get; set; } = [];

    public IReadOnlyList<ReviewItemViewModel> RecentReviews { get; set; } = [];
}

public class ClientNavBadgesViewModel
{
    public int CartCount { get; set; }

    public int FavoritesCount { get; set; }

    public int CompareCount { get; set; }
}

public class SiteFooterViewModel
{
    public IReadOnlyList<CategoryViewModel> Categories { get; set; } = [];

    public IReadOnlyList<string> Brands { get; set; } = [];
}
