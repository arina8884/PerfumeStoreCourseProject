namespace PerfumeStore.MVCC.ViewModels;

public class ReviewListViewModel
{
    public IReadOnlyList<ReviewItemViewModel> Reviews { get; set; } =
        Array.Empty<ReviewItemViewModel>();
}

public class ReviewItemViewModel
{
    public int ReviewId { get; set; }

    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public string UserFullName { get; set; } = string.Empty;

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime ReviewDate { get; set; }

    public bool IsOwnReview { get; set; }
}
