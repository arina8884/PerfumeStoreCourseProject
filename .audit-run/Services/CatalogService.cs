using Microsoft.EntityFrameworkCore;

using PerfumeStore.MVCC.Data;

using PerfumeStore.MVCC.Helpers;

using PerfumeStore.MVCC.Models;

using PerfumeStore.MVCC.Services.Interfaces;

using PerfumeStore.MVCC.ViewModels;



namespace PerfumeStore.MVCC.Services;



public class CatalogService : ICatalogService

{

    private const int LowStockThreshold = 5;



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



        if (!string.IsNullOrWhiteSpace(filter.Brand))

        {

            productsQuery = productsQuery.Where(product => product.Brand == filter.Brand);

        }



        if (filter.Volume.HasValue)

        {

            productsQuery = productsQuery.Where(product => product.Volume == filter.Volume.Value);

        }



        if (filter.MinPrice.HasValue)

        {

            productsQuery = productsQuery.Where(product => product.Price >= filter.MinPrice.Value);

        }



        if (filter.MaxPrice.HasValue)

        {

            productsQuery = productsQuery.Where(product => product.Price <= filter.MaxPrice.Value);

        }



        productsQuery = ApplyStockFilter(productsQuery, filter.StockFilter);



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

                StockQuantity = product.StockQuantity,

                ReviewsCount = product.Reviews.Count,

                AverageRating = product.Reviews.Any()

                    ? product.Reviews.Average(review => (double)review.Rating)

                    : null

            })

            .ToListAsync();



        var activeProductsQuery = _context.Products.AsNoTracking().Where(product => product.IsActive);



        return new ProductCatalogViewModel

        {

            Products = products,

            Categories = await _categoryService.GetAllAsync(),

            Brands = await activeProductsQuery

                .Select(product => product.Brand)

                .Distinct()

                .OrderBy(brand => brand)

                .ToListAsync(),

            Volumes = await activeProductsQuery

                .Select(product => product.Volume)

                .Distinct()

                .OrderBy(volume => volume)

                .ToListAsync(),

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



    private static IQueryable<Product> ApplyStockFilter(IQueryable<Product> query, string? stockFilter)

    {

        return stockFilter switch

        {

            "in_stock" => query.Where(product => product.StockQuantity > LowStockThreshold),

            "low" => query.Where(product => product.StockQuantity > 0 && product.StockQuantity <= LowStockThreshold),

            "out_of_stock" => query.Where(product => product.StockQuantity <= 0),

            _ => query

        };

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

            "stock_desc" => query.OrderByDescending(product => product.StockQuantity),

            _ => query.OrderBy(product => product.Name)

        };

    }

}


