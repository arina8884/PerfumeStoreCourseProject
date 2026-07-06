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
}
