namespace PerfumeStore.MVCC.ViewModels;

public class ProductDetailsViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int Volume { get; set; }

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public int StockQuantity { get; set; }

    public string? ImageUrl { get; set; }

    public double? AverageRating { get; set; }

    public int ReviewsCount { get; set; }

    public bool IsInFavorites { get; set; }

    public bool CanLeaveReview { get; set; }

    public IReadOnlyList<ReviewItemViewModel> Reviews { get; set; } =
        Array.Empty<ReviewItemViewModel>();

    public IReadOnlyList<ProductCardViewModel> SimilarProducts { get; set; } =
        Array.Empty<ProductCardViewModel>();
}
