using Microsoft.EntityFrameworkCore;
using PerfumeStore.MVCC.Constants;
using PerfumeStore.MVCC.Data;
using PerfumeStore.MVCC.Services.Interfaces;
using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Services;

public class ReportService : IReportService
{
    private static readonly string[] AllOrderStatuses =
    [
        OrderStatuses.New,
        OrderStatuses.Confirmed,
        OrderStatuses.Assembling,
        OrderStatuses.InDelivery,
        OrderStatuses.OnTheWay,
        OrderStatuses.Delivered,
        OrderStatuses.Canceled
    ];

    private readonly PerfumeStoreDbContext _context;

    public ReportService(PerfumeStoreDbContext context)
    {
        _context = context;
    }

    public async Task<AdminDashboardViewModel> GetDashboardAsync()
    {
        var usersCount = await _context.Users.AsNoTracking().CountAsync();
        var clientsCount = await _context.Users.AsNoTracking()
            .CountAsync(user => user.Role == UserRoles.Client);
        var managersCount = await _context.Users.AsNoTracking()
            .CountAsync(user => user.Role == UserRoles.OrderManager);
        var productsCount = await _context.Products.AsNoTracking().CountAsync();
        var archivedProductsCount = await _context.Products.AsNoTracking()
            .CountAsync(product => !product.IsActive);

        return new AdminDashboardViewModel
        {
            UsersCount = usersCount,
            ClientsCount = clientsCount,
            ManagersCount = managersCount,
            ProductsCount = productsCount,
            ActiveProductsCount = productsCount - archivedProductsCount,
            ArchivedProductsCount = archivedProductsCount,
            CategoriesCount = await _context.Categories.AsNoTracking().CountAsync(),
            SuppliersCount = await _context.Suppliers.AsNoTracking().CountAsync(),
            OrdersCount = await _context.Orders.AsNoTracking().CountAsync(),
            SupplyRequestsCount = await _context.SupplyRequests.AsNoTracking().CountAsync(),
            RecentUsers = await _context.Users
                .AsNoTracking()
                .OrderByDescending(user => user.CreatedAt)
                .Take(5)
                .Select(user => new UserListItemViewModel
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = user.Role,
                    CreatedAt = user.CreatedAt
                })
                .ToListAsync(),
            RecentOrders = await _context.Orders
                .AsNoTracking()
                .OrderByDescending(order => order.OrderDate)
                .Take(5)
                .Select(order => new ManagerOrderSummaryViewModel
                {
                    OrderId = order.Id,
                    OrderDate = order.OrderDate,
                    ClientFullName = order.User.FullName,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status
                })
                .ToListAsync(),
            RecentSupplyRequests = await _context.SupplyRequests
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
                .ToListAsync()
        };
    }

    public async Task<ReportsViewModel> GetReportsAsync()
    {
        var usersQuery = _context.Users.AsNoTracking();
        var productsQuery = _context.Products.AsNoTracking();

        var orderStatusCounts = await _context.Orders
            .AsNoTracking()
            .GroupBy(order => order.Status)
            .Select(group => new { group.Key, Count = group.Count() })
            .ToListAsync();

        var ordersByStatus = AllOrderStatuses.ToDictionary(
            status => status,
            status => orderStatusCounts.FirstOrDefault(item => item.Key == status)?.Count ?? 0,
            StringComparer.Ordinal);

        var priceStats = await productsQuery
            .GroupBy(_ => 1)
            .Select(group => new
            {
                Min = (decimal?)group.Min(product => product.Price),
                Max = (decimal?)group.Max(product => product.Price),
                Avg = (decimal?)group.Average(product => product.Price)
            })
            .FirstOrDefaultAsync();

        return new ReportsViewModel
        {
            GeneratedAt = DateTime.Now,
            UsersCount = await usersQuery.CountAsync(),
            OrdersCount = await _context.Orders.AsNoTracking().CountAsync(),
            ProductsCount = await productsQuery.CountAsync(),
            SuppliersCount = await _context.Suppliers.AsNoTracking().CountAsync(),
            SupplyRequestsCount = await _context.SupplyRequests.AsNoTracking().CountAsync(),
            CategoriesCount = await _context.Categories.AsNoTracking().CountAsync(),
            ClientsCount = await usersQuery.CountAsync(user => user.Role == UserRoles.Client),
            ContentManagersCount = await usersQuery.CountAsync(user => user.Role == UserRoles.ContentManager),
            OrderManagersCount = await usersQuery.CountAsync(user => user.Role == UserRoles.OrderManager),
            AdminsCount = await usersQuery.CountAsync(user => user.Role == UserRoles.Admin),
            ActiveProductsCount = await productsQuery.CountAsync(product => product.IsActive),
            InactiveProductsCount = await productsQuery.CountAsync(product => !product.IsActive),
            ProductsInStockCount = await productsQuery.CountAsync(product => product.StockQuantity > 0),
            ProductsOutOfStockCount = await productsQuery.CountAsync(product => product.StockQuantity <= 0),
            MinProductPrice = priceStats?.Min,
            MaxProductPrice = priceStats?.Max,
            AverageProductPrice = priceStats?.Avg,
            OrdersByStatus = ordersByStatus
        };
    }
}
