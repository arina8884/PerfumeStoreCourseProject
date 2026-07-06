namespace PerfumeStore.MVCC.ViewModels;

public class FavoriteListViewModel
{
    public IReadOnlyList<FavoriteItemViewModel> Items { get; set; } =
        Array.Empty<FavoriteItemViewModel>();
}

public class FavoriteItemViewModel
{
    public int ProductId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Volume { get; set; }

    public string? ImageUrl { get; set; }

    public string CategoryName { get; set; } = string.Empty;
}
