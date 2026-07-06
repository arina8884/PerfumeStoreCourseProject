using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Services.Interfaces;

public interface IInventoryService
{
    Task<WarehouseViewModel> GetWarehouseAsync();
}
