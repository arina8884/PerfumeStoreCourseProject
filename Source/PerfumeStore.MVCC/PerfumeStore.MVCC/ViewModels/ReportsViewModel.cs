namespace PerfumeStore.MVCC.ViewModels;

public class ReportsViewModel
{
    public int UsersCount { get; set; }

    public int OrdersCount { get; set; }

    public int ProductsCount { get; set; }

    public int SuppliersCount { get; set; }

    public int SupplyRequestsCount { get; set; }

    public int CategoriesCount { get; set; }

    public DateTime GeneratedAt { get; set; } = DateTime.Now;

    public int ClientsCount { get; set; }

    public int ContentManagersCount { get; set; }

    public int OrderManagersCount { get; set; }

    public int AdminsCount { get; set; }

    public int ActiveProductsCount { get; set; }

    public int InactiveProductsCount { get; set; }

    public int ProductsInStockCount { get; set; }

    public int ProductsOutOfStockCount { get; set; }

    public decimal? MinProductPrice { get; set; }

    public decimal? MaxProductPrice { get; set; }

    public decimal? AverageProductPrice { get; set; }

    public Dictionary<string, int> OrdersByStatus { get; set; } = new(StringComparer.Ordinal);
}
