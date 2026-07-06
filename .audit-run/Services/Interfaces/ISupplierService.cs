using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Services.Interfaces;

public interface ISupplierService
{
    Task<SupplierListViewModel> GetAllAsync();

    Task<SupplierEditViewModel?> GetForEditAsync(int id);

    Task<SupplierDetailsViewModel?> GetDetailsAsync(int id);

    Task<int> CreateAsync(SupplierEditViewModel model);

    Task<bool> UpdateAsync(SupplierEditViewModel model);
}
