using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Services.Interfaces;

public interface ICategoryService
{
    Task<IReadOnlyList<CategoryViewModel>> GetAllAsync();

    Task<CategoryViewModel?> GetByIdAsync(int id);
}
