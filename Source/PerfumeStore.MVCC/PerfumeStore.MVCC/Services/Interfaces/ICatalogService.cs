using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Services.Interfaces;

public interface ICatalogService
{
    Task<ProductCatalogViewModel> GetCatalogAsync(ProductFilterViewModel filter);

    Task<ProductCatalogViewModel> SearchAsync(string? searchQuery, string? sortBy);

    Task<ProductCatalogViewModel> GetByCategoryAsync(int categoryId, string? sortBy);
}
