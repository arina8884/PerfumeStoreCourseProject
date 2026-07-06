using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Services.Interfaces;

public interface ISupplyRequestService
{
    Task<SupplyRequestListViewModel> GetAllAsync();

    Task<IReadOnlyList<SupplyRequestSummaryViewModel>> GetRecentAsync(int count);

    Task<SupplyRequestDetailsViewModel?> GetDetailsAsync(int id);

    Task<SupplyRequestEditViewModel> GetCreateModelAsync();

    Task<int> CreateAsync(int userId, SupplyRequestEditViewModel model);

    Task<bool> UpdateStatusAsync(int id, string status);
}
