using Microsoft.EntityFrameworkCore;
using PerfumeStore.MVCC.Data;
using PerfumeStore.MVCC.Services.Interfaces;
using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Services;

public class OrderService : IOrderService
{
    private readonly PerfumeStoreDbContext _context;

    public OrderService(PerfumeStoreDbContext context)
    {
        _context = context;
    }

    public async Task<ClientOrderListViewModel> GetUserOrdersAsync(int userId)
    {
        var orders = await _context.Orders
            .AsNoTracking()
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .Select(o => new ClientOrderSummaryViewModel
            {
                OrderId = o.Id,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status
            })
            .ToListAsync();

        return new ClientOrderListViewModel
        {
            Orders = orders
        };
    }

    public async Task<int?> GetOrderOwnerIdAsync(int orderId)
    {
        return await _context.Orders
            .AsNoTracking()
            .Where(o => o.Id == orderId)
            .Select(o => (int?)o.UserId)
            .FirstOrDefaultAsync();
    }

    public async Task<ClientOrderDetailsViewModel?> GetOrderDetailsAsync(int userId, int orderId)
    {
        var order = await _context.Orders
            .AsNoTracking()
            .Where(o => o.Id == orderId && o.UserId == userId)
            .Select(o => new ClientOrderDetailsViewModel
            {
                OrderId = o.Id,
                OrderDate = o.OrderDate,
                Status = o.Status,
                ClientPhone = o.User.Phone,
                DeliveryAddress = o.DeliveryAddress,
                DeliveryMethod = o.DeliveryMethod,
                PaymentMethod = o.PaymentMethod,
                TotalAmount = o.TotalAmount,
                Items = o.OrderItems
                    .Select(i => new ClientOrderItemViewModel
                    {
                        ProductName = i.Product.Name,
                        ImageUrl = i.Product.ImageUrl,
                        Volume = i.Product.Volume,
                        Quantity = i.Quantity,
                        Price = i.Price
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();

        return order;
    }

    public async Task<OrderTrackingViewModel?> GetOrderTrackingAsync(int userId, int orderId)
    {
        return await _context.Orders
            .AsNoTracking()
            .Where(o => o.Id == orderId && o.UserId == userId)
            .Select(o => new OrderTrackingViewModel
            {
                OrderId = o.Id,
                OrderDate = o.OrderDate,
                Status = o.Status
            })
            .FirstOrDefaultAsync();
    }
}
