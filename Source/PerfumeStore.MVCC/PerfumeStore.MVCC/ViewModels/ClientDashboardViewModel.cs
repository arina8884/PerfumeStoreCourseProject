namespace PerfumeStore.MVCC.ViewModels;

public class ClientDashboardViewModel
{
    public string FullName { get; set; } = string.Empty;

    public int OrdersCount { get; set; }

    public int ActiveOrdersCount { get; set; }

    public int DeliveredOrdersCount { get; set; }

    public int CanceledOrdersCount { get; set; }

    public int FavoritesCount { get; set; }

    public int CartItemsCount { get; set; }

    public int ReviewsCount { get; set; }

    public ClientOrderSummaryViewModel? LastOrder { get; set; }

    public IReadOnlyList<ReviewItemViewModel> RecentReviews { get; set; } =
        Array.Empty<ReviewItemViewModel>();

    public IReadOnlyList<ClientActivityItemViewModel> RecentActivities { get; set; } =
        Array.Empty<ClientActivityItemViewModel>();
}
