using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Services.Interfaces;

public interface IProductService
{
    Task<IReadOnlyList<ProductCardViewModel>> GetNewProductsAsync(int count);

    Task<IReadOnlyList<ProductCardViewModel>> GetRecentActiveProductsAsync(int count);

    Task<IReadOnlyList<ProductCardViewModel>> GetOutOfStockProductsAsync(int count);

    Task<IReadOnlyList<ProductCardViewModel>> GetPopularProductsAsync(int count);

    Task<IReadOnlyList<ProductCardViewModel>> GetSimilarProductsAsync(int productId, int categoryId, string brand, int count);

    Task<IReadOnlyList<string>> GetDistinctBrandsAsync(int count);

    Task<ProductDetailsViewModel?> GetDetailsAsync(int id);

    Task<int> GetTotalCountAsync();

    Task<int> GetArchivedCountAsync();

    Task<ProductManagementListViewModel> GetManagementListAsync();

    Task<ArchivedProductsViewModel> GetArchivedProductsAsync();

    Task<ProductEditViewModel?> GetForEditAsync(int id);

    Task<ProductPreviewViewModel?> GetForPreviewAsync(int id);

    Task<int> CreateAsync(ProductEditViewModel model, string? imageUrl);

    Task<bool> UpdateAsync(ProductEditViewModel model, string? imageUrl);

    Task<bool> ArchiveAsync(int id);

    Task<bool> ActivateAsync(int id);
}
