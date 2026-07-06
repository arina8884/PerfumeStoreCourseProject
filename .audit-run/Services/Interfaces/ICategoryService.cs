using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Services.Interfaces;

public interface ICategoryService
{
    Task<IReadOnlyList<CategoryViewModel>> GetAllAsync();

    Task<IReadOnlyList<CategoryViewModel>> GetRecentAsync(int count);

    Task<int> GetCountAsync();

    Task<CategoryEditViewModel?> GetForEditAsync(int id);

    Task<int> CreateAsync(CategoryEditViewModel model);

    Task<bool> UpdateAsync(CategoryEditViewModel model);
}
