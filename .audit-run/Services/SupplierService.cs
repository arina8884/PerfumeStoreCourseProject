using Microsoft.EntityFrameworkCore;
using PerfumeStore.MVCC.Data;
using PerfumeStore.MVCC.Models;
using PerfumeStore.MVCC.Services.Interfaces;
using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Services;

public class SupplierService : ISupplierService
{
    private readonly PerfumeStoreDbContext _context;

    public SupplierService(PerfumeStoreDbContext context)
    {
        _context = context;
    }

    public async Task<SupplierListViewModel> GetAllAsync()
    {
        var suppliers = await _context.Suppliers
            .AsNoTracking()
            .OrderBy(supplier => supplier.Name)
            .Select(supplier => new SupplierItemViewModel
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Email = supplier.Email,
                Phone = supplier.Phone,
                SupplyRequestsCount = supplier.SupplyRequests.Count
            })
            .ToListAsync();

        return new SupplierListViewModel { Suppliers = suppliers };
    }

    public async Task<SupplierEditViewModel?> GetForEditAsync(int id)
    {
        return await _context.Suppliers
            .AsNoTracking()
            .Where(supplier => supplier.Id == id)
            .Select(supplier => new SupplierEditViewModel
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Email = supplier.Email,
                Phone = supplier.Phone
            })
            .FirstOrDefaultAsync();
    }

    public async Task<SupplierDetailsViewModel?> GetDetailsAsync(int id)
    {
        var supplier = await _context.Suppliers
            .AsNoTracking()
            .Where(item => item.Id == id)
            .Select(item => new SupplierDetailsViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Email = item.Email,
                Phone = item.Phone,
                SupplyRequests = item.SupplyRequests
                    .OrderByDescending(request => request.RequestDate)
                    .Select(request => new SupplyRequestSummaryViewModel
                    {
                        Id = request.Id,
                        SupplierName = item.Name,
                        RequestDate = request.RequestDate,
                        Status = request.Status,
                        CreatedByFullName = request.CreatedByUser.FullName
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();

        return supplier;
    }

    public async Task<int> CreateAsync(SupplierEditViewModel model)
    {
        var supplier = new Supplier
        {
            Name = model.Name.Trim(),
            Email = string.IsNullOrWhiteSpace(model.Email) ? null : model.Email.Trim(),
            Phone = string.IsNullOrWhiteSpace(model.Phone) ? null : model.Phone.Trim()
        };

        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();

        return supplier.Id;
    }

    public async Task<bool> UpdateAsync(SupplierEditViewModel model)
    {
        if (model.Id is null)
        {
            return false;
        }

        var supplier = await _context.Suppliers.FirstOrDefaultAsync(item => item.Id == model.Id);

        if (supplier is null)
        {
            return false;
        }

        supplier.Name = model.Name.Trim();
        supplier.Email = string.IsNullOrWhiteSpace(model.Email) ? null : model.Email.Trim();
        supplier.Phone = string.IsNullOrWhiteSpace(model.Phone) ? null : model.Phone.Trim();

        await _context.SaveChangesAsync();

        return true;
    }
}
