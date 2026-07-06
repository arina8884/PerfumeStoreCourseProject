using Microsoft.EntityFrameworkCore;
using PerfumeStore.MVCC.Constants;
using PerfumeStore.MVCC.Data;
using PerfumeStore.MVCC.Models;
using PerfumeStore.MVCC.Services.Interfaces;
using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Services;

public class SupplyRequestService : ISupplyRequestService
{
    private static readonly string[] AllowedStatuses =
    {
        SupplyRequestStatuses.Pending,
        SupplyRequestStatuses.Approved,
        SupplyRequestStatuses.Completed,
        SupplyRequestStatuses.Rejected
    };

    private static readonly Dictionary<string, string[]> AllowedTransitions = new()
    {
        [SupplyRequestStatuses.Pending] = [SupplyRequestStatuses.Approved, SupplyRequestStatuses.Rejected],
        [SupplyRequestStatuses.Approved] = [SupplyRequestStatuses.Completed, SupplyRequestStatuses.Rejected],
        [SupplyRequestStatuses.Completed] = [],
        [SupplyRequestStatuses.Rejected] = []
    };

    private readonly PerfumeStoreDbContext _context;

    public SupplyRequestService(PerfumeStoreDbContext context)
    {
        _context = context;
    }

    public async Task<SupplyRequestListViewModel> GetAllAsync()
    {
        var requests = await _context.SupplyRequests
            .AsNoTracking()
            .OrderByDescending(request => request.RequestDate)
            .Select(request => new SupplyRequestSummaryViewModel
            {
                Id = request.Id,
                SupplierName = request.Supplier.Name,
                RequestDate = request.RequestDate,
                Status = request.Status,
                CreatedByFullName = request.CreatedByUser.FullName
            })
            .ToListAsync();

        return new SupplyRequestListViewModel { Requests = requests };
    }

    public async Task<IReadOnlyList<SupplyRequestSummaryViewModel>> GetRecentAsync(int count)
    {
        return await _context.SupplyRequests
            .AsNoTracking()
            .OrderByDescending(request => request.RequestDate)
            .Take(count)
            .Select(request => new SupplyRequestSummaryViewModel
            {
                Id = request.Id,
                SupplierName = request.Supplier.Name,
                RequestDate = request.RequestDate,
                Status = request.Status,
                CreatedByFullName = request.CreatedByUser.FullName
            })
            .ToListAsync();
    }

    public async Task<SupplyRequestDetailsViewModel?> GetDetailsAsync(int id)
    {
        var request = await _context.SupplyRequests
            .AsNoTracking()
            .Where(item => item.Id == id)
            .Select(item => new SupplyRequestDetailsViewModel
            {
                Id = item.Id,
                SupplierName = item.Supplier.Name,
                CreatedByFullName = item.CreatedByUser.FullName,
                RequestDate = item.RequestDate,
                Status = item.Status,
                Comment = item.Comment,
                Items = item.SupplyRequestItems
                    .Select(line => new SupplyRequestItemViewModel
                    {
                        ProductId = line.ProductId,
                        ProductName = line.Product.Name,
                        Quantity = line.Quantity
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();

        if (request is null)
        {
            return null;
        }

        request.AvailableStatuses = GetAllowedTransitions(request.Status);

        return request;
    }

    public async Task<SupplyRequestEditViewModel> GetCreateModelAsync()
    {
        var suppliers = await _context.Suppliers
            .AsNoTracking()
            .OrderBy(supplier => supplier.Name)
            .Select(supplier => new SupplierItemViewModel
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Email = supplier.Email,
                Phone = supplier.Phone
            })
            .ToListAsync();

        var products = await _context.Products
            .AsNoTracking()
            .Where(product => product.IsActive)
            .OrderBy(product => product.Name)
            .Select(product => new ProductSelectViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Brand = product.Brand
            })
            .ToListAsync();

        return new SupplyRequestEditViewModel
        {
            Suppliers = suppliers,
            Products = products,
            Items =
            [
                new SupplyRequestLineEditViewModel(),
                new SupplyRequestLineEditViewModel(),
                new SupplyRequestLineEditViewModel()
            ]
        };
    }

    public async Task<int> CreateAsync(int userId, SupplyRequestEditViewModel model)
    {
        var validItems = model.Items
            .Where(item => item.ProductId > 0 && item.Quantity > 0)
            .GroupBy(item => item.ProductId)
            .Select(group => new SupplyRequestItem
            {
                ProductId = group.Key,
                Quantity = group.Sum(item => item.Quantity)
            })
            .ToList();

        if (validItems.Count == 0)
        {
            throw new InvalidOperationException("Добавьте хотя бы одну позицию в заявку.");
        }

        var supplierExists = await _context.Suppliers
            .AsNoTracking()
            .AnyAsync(supplier => supplier.Id == model.SupplierId);

        if (!supplierExists)
        {
            throw new InvalidOperationException("Поставщик не найден.");
        }

        var request = new SupplyRequest
        {
            SupplierId = model.SupplierId,
            CreatedByUserId = userId,
            Status = SupplyRequestStatuses.Pending,
            Comment = string.IsNullOrWhiteSpace(model.Comment) ? null : model.Comment.Trim(),
            SupplyRequestItems = validItems
        };

        _context.SupplyRequests.Add(request);
        await _context.SaveChangesAsync();

        return request.Id;
    }

    public async Task<bool> UpdateStatusAsync(int id, string status)
    {
        if (!AllowedStatuses.Contains(status))
        {
            return false;
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();

        var request = await _context.SupplyRequests
            .Include(item => item.SupplyRequestItems)
            .ThenInclude(line => line.Product)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (request is null || !IsTransitionAllowed(request.Status, status))
        {
            return false;
        }

        if (status == SupplyRequestStatuses.Completed)
        {
            IncreaseStock(request);
        }

        request.Status = status;
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return true;
    }

    private static void IncreaseStock(SupplyRequest request)
    {
        foreach (var item in request.SupplyRequestItems)
        {
            item.Product.StockQuantity += item.Quantity;
        }
    }

    private static IReadOnlyList<string> GetAllowedTransitions(string currentStatus)
    {
        return AllowedTransitions.TryGetValue(currentStatus, out var transitions)
            ? transitions
            : Array.Empty<string>();
    }

    private static bool IsTransitionAllowed(string currentStatus, string newStatus)
    {
        return GetAllowedTransitions(currentStatus).Contains(newStatus);
    }
}
