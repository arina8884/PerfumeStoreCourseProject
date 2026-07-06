using Microsoft.EntityFrameworkCore;
using PerfumeStore.MVCC.Data;
using PerfumeStore.MVCC.Services.Interfaces;
using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Services;

public class InventoryService : IInventoryService
{
    private readonly PerfumeStoreDbContext _context;

    public InventoryService(PerfumeStoreDbContext context)
    {
        _context = context;
    }

    public async Task<WarehouseViewModel> GetWarehouseAsync()
    {
        var items = await _context.Products
            .AsNoTracking()
            .OrderBy(product => product.Name)
            .Select(product => new WarehouseItemViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Brand = product.Brand,
                CategoryName = product.Category.Name,
                StockQuantity = product.StockQuantity,
                IsActive = product.IsActive,
                ImageUrl = product.ImageUrl
            })
            .ToListAsync();

        return new WarehouseViewModel
        {
            TotalProducts = items.Count,
            ActiveProducts = items.Count(item => item.IsActive),
            LowStockCount = items.Count(item => item.IsActive && item.StockQuantity > 0 && item.StockQuantity <= 5),
            OutOfStockCount = items.Count(item => item.IsActive && item.StockQuantity <= 0),
            Items = items
        };
    }
}
