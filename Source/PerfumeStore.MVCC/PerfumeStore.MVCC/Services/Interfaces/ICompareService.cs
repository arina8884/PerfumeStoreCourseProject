using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Services.Interfaces;

public interface ICompareService
{
    Task<CompareViewModel> GetCompareAsync();

    Task<int> GetCountAsync();

    Task<bool> AddProductAsync(int productId);

    Task RemoveProductAsync(int productId);

    Task ClearAsync();
}
