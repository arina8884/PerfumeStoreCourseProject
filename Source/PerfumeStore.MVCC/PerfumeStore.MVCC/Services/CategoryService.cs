using Microsoft.EntityFrameworkCore;
using PerfumeStore.MVCC.Data;
using PerfumeStore.MVCC.Services.Interfaces;
using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Services;

public class CategoryService : ICategoryService
{
    private readonly PerfumeStoreDbContext _context;

    public CategoryService(PerfumeStoreDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<CategoryViewModel>> GetAllAsync()
    {
        return await _context.Categories
            .AsNoTracking()
            .OrderBy(category => category.Name)
            .Select(category => new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            })
            .ToListAsync();
    }

    public async Task<CategoryViewModel?> GetByIdAsync(int id)
    {
        return await _context.Categories
            .AsNoTracking()
            .Where(category => category.Id == id)
            .Select(category => new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            })
            .FirstOrDefaultAsync();
    }
}
