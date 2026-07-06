namespace PerfumeStore.MVCC.ViewModels;

public class OrderManagerDashboardViewModel
{
    public int NewOrdersCount { get; set; }

    public int InProgressOrdersCount { get; set; }

    public int CompletedOrdersCount { get; set; }

    public int SuppliersCount { get; set; }

    public int ActiveSupplyRequestsCount { get; set; }

    public int OutOfStockCount { get; set; }

    public int LowStockCount { get; set; }

    public IReadOnlyList<ManagerOrderSummaryViewModel> QueueOrders { get; set; } =
        Array.Empty<ManagerOrderSummaryViewModel>();

    public IReadOnlyList<ProductCardViewModel> OutOfStockProducts { get; set; } =
        Array.Empty<ProductCardViewModel>();

    public IReadOnlyList<ManagerOrderSummaryViewModel> RecentOrders { get; set; } =
        Array.Empty<ManagerOrderSummaryViewModel>();

    public IReadOnlyList<SupplyRequestSummaryViewModel> RecentSupplyRequests { get; set; } =
        Array.Empty<SupplyRequestSummaryViewModel>();

    public IReadOnlyList<ProductCardViewModel> LowStockProducts { get; set; } =
        Array.Empty<ProductCardViewModel>();
}
