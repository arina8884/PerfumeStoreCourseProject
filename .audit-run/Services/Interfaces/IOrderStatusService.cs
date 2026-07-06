using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Services.Interfaces;

public interface IOrderStatusService
{
    Task<OrderManagerDashboardViewModel> GetDashboardAsync();

    Task<OrderQueueViewModel> GetAllOrdersAsync();

    Task<OrderQueueViewModel> GetQueueAsync();

    Task<ManagerOrderHistoryViewModel> GetHistoryAsync();

    Task<ManagerOrderDetailsViewModel?> GetOrderDetailsAsync(int orderId);

    Task<OrderStatusUpdateViewModel?> GetStatusUpdateAsync(int orderId);

    Task<OrderCancelViewModel?> GetCancelAsync(int orderId);

    Task<bool> UpdateStatusAsync(int orderId, string status);

    Task<bool> CancelOrderAsync(int orderId);
}
