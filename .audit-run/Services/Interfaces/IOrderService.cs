using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Services.Interfaces;

public interface IOrderService
{
    Task<ClientOrderListViewModel> GetUserOrdersAsync(int userId);

    Task<ClientOrderDetailsViewModel?> GetOrderDetailsAsync(int userId, int orderId);

    Task<OrderTrackingViewModel?> GetOrderTrackingAsync(int userId, int orderId);

    Task<int?> GetOrderOwnerIdAsync(int orderId);
}
