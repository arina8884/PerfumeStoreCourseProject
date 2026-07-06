using Microsoft.EntityFrameworkCore;
using PerfumeStore.MVCC.Constants;
using PerfumeStore.MVCC.Data;
using PerfumeStore.MVCC.Helpers;
using PerfumeStore.MVCC.Models;
using PerfumeStore.MVCC.Services.Interfaces;
using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Services;

public class OrderStatusService : IOrderStatusService
{
    private static readonly string[] InProgressStatuses =
    {
        OrderStatuses.Confirmed,
        OrderStatuses.Assembling,
        OrderStatuses.InDelivery,
        OrderStatuses.OnTheWay
    };

    private static readonly string[] HistoryStatuses =
    {
        OrderStatuses.Delivered,
        OrderStatuses.Canceled
    };

    private readonly PerfumeStoreDbContext _context;

    public OrderStatusService(PerfumeStoreDbContext context)
    {
        _context = context;
    }

    public async Task<OrderManagerDashboardViewModel> GetDashboardAsync()
    {
        var newOrdersCount = await _context.Orders
            .AsNoTracking()
            .CountAsync(order => order.Status == OrderStatuses.New);

        var inProgressOrdersCount = await _context.Orders
            .AsNoTracking()
            .CountAsync(order => InProgressStatuses.Contains(order.Status));

        var completedOrdersCount = await _context.Orders
            .AsNoTracking()
            .CountAsync(order => order.Status == OrderStatuses.Delivered);

        var suppliersCount = await _context.Suppliers.AsNoTracking().CountAsync();

        var activeSupplyRequestsCount = await _context.SupplyRequests
            .AsNoTracking()
            .CountAsync(request =>
                request.Status == SupplyRequestStatuses.Pending ||
                request.Status == SupplyRequestStatuses.Approved);

        var outOfStockCount = await _context.Products
            .AsNoTracking()
            .CountAsync(product => product.IsActive && product.StockQuantity <= 0);

        var lowStockCount = await _context.Products
            .AsNoTracking()
            .CountAsync(product => product.IsActive && product.StockQuantity > 0 && product.StockQuantity <= 5);

        var queueOrders = await BuildOrderSummaryQuery(
                _context.Orders.AsNoTracking().Where(order => order.Status == OrderStatuses.New))
            .OrderBy(order => order.OrderDate)
            .Take(5)
            .ToListAsync();

        var outOfStockProducts = await _context.Products
            .AsNoTracking()
            .Where(product => product.IsActive && product.StockQuantity <= 0)
            .OrderBy(product => product.Name)
            .Take(5)
            .Select(product => new ProductCardViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Brand = product.Brand,
                Price = product.Price,
                Volume = product.Volume,
                ImageUrl = product.ImageUrl,
                CategoryName = product.Category.Name,
                StockQuantity = product.StockQuantity
            })
            .ToListAsync();

        var lowStockProducts = await _context.Products
            .AsNoTracking()
            .Where(product => product.IsActive && product.StockQuantity > 0 && product.StockQuantity <= 5)
            .OrderBy(product => product.StockQuantity)
            .ThenBy(product => product.Name)
            .Take(5)
            .Select(product => new ProductCardViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Brand = product.Brand,
                Price = product.Price,
                Volume = product.Volume,
                ImageUrl = product.ImageUrl,
                CategoryName = product.Category.Name,
                StockQuantity = product.StockQuantity
            })
            .ToListAsync();

        var recentOrders = await BuildOrderSummaryQuery(_context.Orders.AsNoTracking())
            .OrderByDescending(order => order.OrderDate)
            .Take(5)
            .ToListAsync();

        var recentSupplyRequests = await _context.SupplyRequests
            .AsNoTracking()
            .OrderByDescending(request => request.RequestDate)
            .Take(5)
            .Select(request => new SupplyRequestSummaryViewModel
            {
                Id = request.Id,
                SupplierName = request.Supplier.Name,
                RequestDate = request.RequestDate,
                Status = request.Status,
                CreatedByFullName = request.CreatedByUser.FullName
            })
            .ToListAsync();

        return new OrderManagerDashboardViewModel
        {
            NewOrdersCount = newOrdersCount,
            InProgressOrdersCount = inProgressOrdersCount,
            CompletedOrdersCount = completedOrdersCount,
            SuppliersCount = suppliersCount,
            ActiveSupplyRequestsCount = activeSupplyRequestsCount,
            OutOfStockCount = outOfStockCount,
            LowStockCount = lowStockCount,
            QueueOrders = queueOrders,
            OutOfStockProducts = outOfStockProducts,
            LowStockProducts = lowStockProducts,
            RecentOrders = recentOrders,
            RecentSupplyRequests = recentSupplyRequests
        };
    }

    public async Task<OrderQueueViewModel> GetAllOrdersAsync()
    {
        var orders = await BuildOrderSummaryQuery(_context.Orders.AsNoTracking())
            .OrderByDescending(order => order.OrderDate)
            .ToListAsync();

        return new OrderQueueViewModel { Orders = orders };
    }

    public async Task<OrderQueueViewModel> GetQueueAsync()
    {
        var orders = await BuildOrderSummaryQuery(
                _context.Orders.AsNoTracking().Where(order => order.Status == OrderStatuses.New))
            .OrderBy(order => order.OrderDate)
            .ToListAsync();

        return new OrderQueueViewModel { Orders = orders };
    }

    public async Task<ManagerOrderHistoryViewModel> GetHistoryAsync()
    {
        var orders = await BuildOrderSummaryQuery(
                _context.Orders.AsNoTracking().Where(order => HistoryStatuses.Contains(order.Status)))
            .OrderByDescending(order => order.OrderDate)
            .ToListAsync();

        return new ManagerOrderHistoryViewModel { Orders = orders };
    }

    public async Task<ManagerOrderDetailsViewModel?> GetOrderDetailsAsync(int orderId)
    {
        return await _context.Orders
            .AsNoTracking()
            .Where(order => order.Id == orderId)
            .Select(order => new ManagerOrderDetailsViewModel
            {
                OrderId = order.Id,
                OrderDate = order.OrderDate,
                Status = order.Status,
                ClientFullName = order.User.FullName,
                ClientEmail = order.User.Email,
                ClientPhone = order.User.Phone,
                DeliveryAddress = order.DeliveryAddress,
                DeliveryMethod = order.DeliveryMethod,
                PaymentMethod = order.PaymentMethod,
                TotalAmount = order.TotalAmount,
                Items = order.OrderItems
                    .Select(item => new ManagerOrderItemViewModel
                    {
                        ProductName = item.Product.Name,
                        ImageUrl = item.Product.ImageUrl,
                        Volume = item.Product.Volume,
                        Quantity = item.Quantity,
                        Price = item.Price
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<OrderStatusUpdateViewModel?> GetStatusUpdateAsync(int orderId)
    {
        var order = await _context.Orders
            .AsNoTracking()
            .Where(item => item.Id == orderId)
            .Select(item => new { item.Id, item.Status })
            .FirstOrDefaultAsync();

        if (order is null)
        {
            return null;
        }

        var allowedTransitions = OrderStatusHelper.GetAllowedTransitions(order.Status);

        return new OrderStatusUpdateViewModel
        {
            OrderId = order.Id,
            CurrentStatus = order.Status,
            Status = allowedTransitions.FirstOrDefault() ?? order.Status,
            AvailableStatuses = allowedTransitions
        };
    }

    public async Task<OrderCancelViewModel?> GetCancelAsync(int orderId)
    {
        return await _context.Orders
            .AsNoTracking()
            .Where(order => order.Id == orderId)
            .Select(order => new OrderCancelViewModel
            {
                OrderId = order.Id,
                Status = order.Status,
                ClientFullName = order.User.FullName,
                TotalAmount = order.TotalAmount
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateStatusAsync(int orderId, string status)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        var order = await LoadOrderForStatusChangeAsync(orderId);

        if (order is null)
        {
            return false;
        }

        if (!OrderStatusHelper.IsTransitionAllowed(order.Status, status))
        {
            return false;
        }

        if (status == OrderStatuses.Canceled && order.Status != OrderStatuses.Canceled)
        {
            RestoreStock(order);
        }

        order.Status = status;
        order.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return true;
    }

    public async Task<bool> CancelOrderAsync(int orderId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        var order = await LoadOrderForStatusChangeAsync(orderId);

        if (order is null || !OrderStatusHelper.IsCancellationAllowed(order.Status))
        {
            return false;
        }

        RestoreStock(order);

        order.Status = OrderStatuses.Canceled;
        order.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return true;
    }

    private async Task<Order?> LoadOrderForStatusChangeAsync(int orderId)
    {
        return await _context.Orders
            .Include(order => order.OrderItems)
            .ThenInclude(item => item.Product)
            .FirstOrDefaultAsync(order => order.Id == orderId);
    }

    private static void RestoreStock(Order order)
    {
        foreach (var item in order.OrderItems)
        {
            item.Product.StockQuantity += item.Quantity;
        }
    }

    private IQueryable<ManagerOrderSummaryViewModel> BuildOrderSummaryQuery(IQueryable<Order> query)
    {
        return query.Select(order => new ManagerOrderSummaryViewModel
        {
            OrderId = order.Id,
            OrderDate = order.OrderDate,
            ClientFullName = order.User.FullName,
            TotalAmount = order.TotalAmount,
            Status = order.Status
        });
    }
}
