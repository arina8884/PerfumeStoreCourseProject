namespace PerfumeStore.MVCC.ViewModels;

public class AdminDashboardViewModel
{
    public int UsersCount { get; set; }

    public int ClientsCount { get; set; }

    public int ManagersCount { get; set; }

    public int ProductsCount { get; set; }

    public int ActiveProductsCount { get; set; }

    public int ArchivedProductsCount { get; set; }

    public int CategoriesCount { get; set; }

    public int SuppliersCount { get; set; }

    public int OrdersCount { get; set; }

    public int SupplyRequestsCount { get; set; }

    public IReadOnlyList<UserListItemViewModel> RecentUsers { get; set; } =
        Array.Empty<UserListItemViewModel>();

    public IReadOnlyList<ManagerOrderSummaryViewModel> RecentOrders { get; set; } =
        Array.Empty<ManagerOrderSummaryViewModel>();

    public IReadOnlyList<SupplyRequestSummaryViewModel> RecentSupplyRequests { get; set; } =
        Array.Empty<SupplyRequestSummaryViewModel>();
}
