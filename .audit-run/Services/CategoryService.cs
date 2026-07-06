using Microsoft.EntityFrameworkCore;
using PerfumeStore.MVCC.Data;
using PerfumeStore.MVCC.Models;
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

    public async Task<IReadOnlyList<CategoryViewModel>> GetRecentAsync(int count)
    {
        return await _context.Categories
            .AsNoTracking()
            .OrderByDescending(category => category.Id)
            .Take(count)
            .Select(category => new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            })
            .ToListAsync();
    }

    public Task<int> GetCountAsync()
    {
        return _context.Categories.AsNoTracking().CountAsync();
    }

    public async Task<CategoryEditViewModel?> GetForEditAsync(int id)
    {
        return await _context.Categories
            .AsNoTracking()
            .Where(category => category.Id == id)
            .Select(category => new CategoryEditViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            })
            .FirstOrDefaultAsync();
    }

    public async Task<int> CreateAsync(CategoryEditViewModel model)
    {
        var category = new Category
        {
            Name = model.Name.Trim(),
            Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim()
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return category.Id;
    }

    public async Task<bool> UpdateAsync(CategoryEditViewModel model)
    {
        if (model.Id is null)
        {
            return false;
        }

        var category = await _context.Categories.FirstOrDefaultAsync(item => item.Id == model.Id);

        if (category is null)
        {
            return false;
        }

        category.Name = model.Name.Trim();
        category.Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim();

        await _context.SaveChangesAsync();

        return true;
    }
}
