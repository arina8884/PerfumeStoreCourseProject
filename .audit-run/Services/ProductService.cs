using Microsoft.EntityFrameworkCore;
using PerfumeStore.MVCC.Data;
using PerfumeStore.MVCC.Models;
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

    public async Task<IReadOnlyList<ProductCardViewModel>> GetNewProductsAsync(int count)
    {
        return await BuildActiveProductCardsQuery()
            .OrderByDescending(product => product.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public Task<IReadOnlyList<ProductCardViewModel>> GetRecentActiveProductsAsync(int count)
    {
        return GetNewProductsAsync(count);
    }

    public async Task<IReadOnlyList<ProductCardViewModel>> GetOutOfStockProductsAsync(int count)
    {
        return await _context.Products
            .AsNoTracking()
            .Where(product => product.IsActive && product.StockQuantity <= 0)
            .OrderBy(product => product.Name)
            .Take(count)
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
    }

    public async Task<IReadOnlyList<ProductCardViewModel>> GetPopularProductsAsync(int count)
    {
        return await BuildActiveProductCardsQuery()
            .OrderByDescending(product => product.StockQuantity)
            .ThenBy(product => product.Name)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<ProductCardViewModel>> GetSimilarProductsAsync(
        int productId,
        int categoryId,
        string brand,
        int count)
    {
        return await _context.Products
            .AsNoTracking()
            .Where(product => product.IsActive
                && product.Id != productId
                && (product.CategoryId == categoryId || product.Brand == brand))
            .OrderByDescending(product => product.CategoryId == categoryId)
            .ThenByDescending(product => product.Brand == brand)
            .ThenBy(product => product.Name)
            .Take(count)
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
                CreatedAt = product.CreatedAt,
                ReviewsCount = product.Reviews.Count,
                AverageRating = product.Reviews.Any()
                    ? product.Reviews.Average(review => (double)review.Rating)
                    : null
            })
            .ToListAsync();
    }

    public async Task<IReadOnlyList<string>> GetDistinctBrandsAsync(int count)
    {
        return await _context.Products
            .AsNoTracking()
            .Where(product => product.IsActive)
            .Select(product => product.Brand)
            .Distinct()
            .OrderBy(brand => brand)
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
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name,
                StockQuantity = product.StockQuantity,
                ImageUrl = product.ImageUrl
            })
            .FirstOrDefaultAsync();
    }

    public Task<int> GetTotalCountAsync()
    {
        return _context.Products.AsNoTracking().CountAsync();
    }

    public Task<int> GetArchivedCountAsync()
    {
        return _context.Products.AsNoTracking().CountAsync(product => !product.IsActive);
    }

    public async Task<ProductManagementListViewModel> GetManagementListAsync()
    {
        var products = await BuildManagementItemQuery(_context.Products.AsNoTracking())
            .Where(product => product.IsActive)
            .OrderBy(product => product.Name)
            .ToListAsync();

        return new ProductManagementListViewModel { Products = products };
    }

    public async Task<ArchivedProductsViewModel> GetArchivedProductsAsync()
    {
        var products = await _context.Products
            .AsNoTracking()
            .Where(product => !product.IsActive)
            .OrderBy(product => product.Name)
            .Select(product => new ProductManagementItemViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Brand = product.Brand,
                CategoryName = product.Category.Name,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                IsActive = product.IsActive,
                ImageUrl = product.ImageUrl
            })
            .ToListAsync();

        return new ArchivedProductsViewModel { Products = products };
    }

    public async Task<ProductEditViewModel?> GetForEditAsync(int id)
    {
        var product = await _context.Products
            .AsNoTracking()
            .Where(item => item.Id == id)
            .Select(item => new ProductEditViewModel
            {
                Id = item.Id,
                CategoryId = item.CategoryId,
                Name = item.Name,
                Brand = item.Brand,
                Price = item.Price,
                Volume = item.Volume,
                Description = item.Description,
                StockQuantity = item.StockQuantity,
                ImageUrl = item.ImageUrl
            })
            .FirstOrDefaultAsync();

        if (product is null)
        {
            return null;
        }

        product.Categories = await GetCategoryOptionsAsync();

        return product;
    }

    public async Task<ProductPreviewViewModel?> GetForPreviewAsync(int id)
    {
        return await _context.Products
            .AsNoTracking()
            .Where(product => product.Id == id)
            .Select(product => new ProductPreviewViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Brand = product.Brand,
                CategoryName = product.Category.Name,
                Price = product.Price,
                Volume = product.Volume,
                Description = product.Description,
                StockQuantity = product.StockQuantity,
                ImageUrl = product.ImageUrl,
                IsActive = product.IsActive
            })
            .FirstOrDefaultAsync();
    }

    public async Task<int> CreateAsync(ProductEditViewModel model, string? imageUrl)
    {
        var product = new Product
        {
            CategoryId = model.CategoryId,
            Name = model.Name.Trim(),
            Brand = model.Brand.Trim(),
            Price = model.Price,
            Volume = model.Volume,
            Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim(),
            StockQuantity = model.StockQuantity,
            ImageUrl = imageUrl,
            IsActive = true
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return product.Id;
    }

    public async Task<bool> UpdateAsync(ProductEditViewModel model, string? imageUrl)
    {
        if (model.Id is null)
        {
            return false;
        }

        var product = await _context.Products.FirstOrDefaultAsync(item => item.Id == model.Id);

        if (product is null)
        {
            return false;
        }

        product.CategoryId = model.CategoryId;
        product.Name = model.Name.Trim();
        product.Brand = model.Brand.Trim();
        product.Price = model.Price;
        product.Volume = model.Volume;
        product.Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim();
        product.StockQuantity = model.StockQuantity;

        if (!string.IsNullOrWhiteSpace(imageUrl))
        {
            product.ImageUrl = imageUrl;
        }

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ArchiveAsync(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(item => item.Id == id);

        if (product is null || !product.IsActive)
        {
            return false;
        }

        product.IsActive = false;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ActivateAsync(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(item => item.Id == id);

        if (product is null || product.IsActive)
        {
            return false;
        }

        product.IsActive = true;
        await _context.SaveChangesAsync();

        return true;
    }

    private async Task<IReadOnlyList<CategoryViewModel>> GetCategoryOptionsAsync()
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
                CreatedAt = product.CreatedAt,
                ReviewsCount = product.Reviews.Count,
                AverageRating = product.Reviews.Any()
                    ? product.Reviews.Average(review => (double)review.Rating)
                    : null
            });
    }

    private IQueryable<ProductManagementItemViewModel> BuildManagementItemQuery(IQueryable<Product> query)
    {
        return query.Select(product => new ProductManagementItemViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Brand = product.Brand,
            CategoryName = product.Category.Name,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            IsActive = product.IsActive,
            ImageUrl = product.ImageUrl
        });
    }
}
