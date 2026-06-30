using Microsoft.EntityFrameworkCore;
using PerfumeStore.MVCC.Data;
using PerfumeStore.MVCC.Models;
using PerfumeStore.MVCC.Services.Interfaces;
using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Services;

public class CatalogService : ICatalogService
{
    private readonly PerfumeStoreDbContext _context;
    private readonly ICategoryService _categoryService;

    public CatalogService(PerfumeStoreDbContext context, ICategoryService categoryService)
    {
        _context = context;
        _categoryService = categoryService;
    }

    public async Task<ProductCatalogViewModel> GetCatalogAsync(ProductFilterViewModel filter)
    {
        var productsQuery = BuildCatalogQuery();

        if (!string.IsNullOrWhiteSpace(filter.SearchQuery))
        {
            var searchQuery = filter.SearchQuery.Trim().ToLower();
            productsQuery = productsQuery.Where(product =>
                product.Name.ToLower().Contains(searchQuery) ||
                product.Brand.ToLower().Contains(searchQuery));
        }

        if (filter.CategoryId.HasValue)
        {
            productsQuery = productsQuery.Where(product => product.CategoryId == filter.CategoryId.Value);
        }

        var products = await ApplySorting(productsQuery, filter.SortBy)
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

        return new ProductCatalogViewModel
        {
            Products = products,
            Categories = await _categoryService.GetAllAsync(),
            Filter = filter
        };
    }

    public Task<ProductCatalogViewModel> SearchAsync(string? searchQuery, string? sortBy)
    {
        return GetCatalogAsync(new ProductFilterViewModel
        {
            SearchQuery = searchQuery,
            SortBy = sortBy
        });
    }

    public Task<ProductCatalogViewModel> GetByCategoryAsync(int categoryId, string? sortBy)
    {
        return GetCatalogAsync(new ProductFilterViewModel
        {
            CategoryId = categoryId,
            SortBy = sortBy
        });
    }

    private IQueryable<Product> BuildCatalogQuery()
    {
        return _context.Products
            .AsNoTracking()
            .Where(product => product.IsActive);
    }

    private static IQueryable<Product> ApplySorting(
        IQueryable<Product> query,
        string? sortBy)
    {
        return sortBy switch
        {
            "price_asc" => query.OrderBy(product => product.Price),
            "price_desc" => query.OrderByDescending(product => product.Price),
            "name" => query.OrderBy(product => product.Name),
            "newest" => query.OrderByDescending(product => product.CreatedAt),
            _ => query.OrderBy(product => product.Name)
        };
    }
}
