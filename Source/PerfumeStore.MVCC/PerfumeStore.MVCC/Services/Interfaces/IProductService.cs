using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Services.Interfaces;

public interface IProductService
{
    Task<IReadOnlyList<ProductCardViewModel>> GetActiveProductsAsync();

    Task<IReadOnlyList<ProductCardViewModel>> GetNewProductsAsync(int count);

    Task<IReadOnlyList<ProductCardViewModel>> GetPopularProductsAsync(int count);

    Task<ProductDetailsViewModel?> GetDetailsAsync(int id);
}
