namespace PerfumeStore.MVCC.ViewModels;

public class ArchivedProductsViewModel
{
    public IReadOnlyList<ProductManagementItemViewModel> Products { get; set; } =
        Array.Empty<ProductManagementItemViewModel>();
}
