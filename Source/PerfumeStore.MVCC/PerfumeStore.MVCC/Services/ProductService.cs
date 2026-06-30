using Microsoft.EntityFrameworkCore;
using PerfumeStore.MVCC.Data;
using PerfumeStore.MVCC.Services.Interfaces;
using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Services;

public class ProductService : IProductService
{
    private readonly PerfumeStoreDbContext _context;

    public ProductService(PerfumeStoreDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<ProductCardViewModel>> GetActiveProductsAsync()
    {
        return await BuildActiveProductCardsQuery()
            .OrderBy(product => product.Name)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<ProductCardViewModel>> GetNewProductsAsync(int count)
    {
        return await BuildActiveProductCardsQuery()
            .OrderByDescending(product => product.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<ProductCardViewModel>> GetPopularProductsAsync(int count)
    {
        return await BuildActiveProductCardsQuery()
            .OrderByDescending(product => product.StockQuantity)
            .ThenBy(product => product.Name)
            .Take(count)
            .ToListAsync();
    }

    public async Task<ProductDetailsViewModel?> GetDetailsAsync(int id)
    {
        return await _context.Products
            .AsNoTracking()
            .Where(product => product.Id == id && product.IsActive)
            .Select(product => new ProductDetailsViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Brand = product.Brand,
                Description = product.Description,
                Price = product.Price,
                Volume = product.Volume,
                CategoryName = product.Category.Name,
                StockQuantity = product.StockQuantity,
                ImageUrl = product.ImageUrl
            })
            .FirstOrDefaultAsync();
    }

    private IQueryable<ProductCardViewModel> BuildActiveProductCardsQuery()
    {
        return _context.Products
            .AsNoTracking()
            .Where(product => product.IsActive)
            .Select(product => new ProductCardViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Brand = product.Brand,
                Price = product.Price,
                Volume = product.Volume,
                ImageUrl = product.ImageUrl,
                CategoryName = product.Category.Name,
                StockQuantity = product.StockQuantity,
                CreatedAt = product.CreatedAt
            });
    }
}
