using Microsoft.EntityFrameworkCore;
using PerfumeStore.MVCC.Data;
using PerfumeStore.MVCC.Helpers;
using PerfumeStore.MVCC.Services.Interfaces;
using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Services;

public class CompareService : ICompareService
{
    private const string SessionKey = "CompareProductIds";
    private const int MaxProducts = 2;

    private readonly PerfumeStoreDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CompareService(PerfumeStoreDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CompareViewModel> GetCompareAsync()
    {
        var ids = GetStoredIds();

        if (ids.Count == 0)
        {
            return new CompareViewModel();
        }

        var products = await _context.Products
            .AsNoTracking()
            .Where(product => ids.Contains(product.Id) && product.IsActive)
            .Select(product => new ProductCompareItemViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Brand = product.Brand,
                CategoryName = product.Category.Name,
                Volume = product.Volume,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                ReviewsCount = product.Reviews.Count,
                AverageRating = product.Reviews.Any()
                    ? product.Reviews.Average(review => (double)review.Rating)
                    : null
            })
            .ToListAsync();

        foreach (var product in products)
        {
            product.StockStatusLabel = StockStatusHelper.ToDisplayName(product.StockQuantity);
        }

        products = ids
            .Select(id => products.FirstOrDefault(product => product.Id == id))
            .Where(product => product is not null)
            .Cast<ProductCompareItemViewModel>()
            .ToList();

        SaveIds(products.Select(product => product.Id).ToList());

        return new CompareViewModel { Products = products };
    }

    public async Task<bool> AddProductAsync(int productId)
    {
        var exists = await _context.Products
            .AsNoTracking()
            .AnyAsync(product => product.Id == productId && product.IsActive);

        if (!exists)
        {
            return false;
        }

        var ids = GetStoredIds();

        if (ids.Contains(productId))
        {
            return true;
        }

        if (ids.Count >= MaxProducts)
        {
            ids.RemoveAt(0);
        }

        ids.Add(productId);
        SaveIds(ids);

        return true;
    }

    public Task RemoveProductAsync(int productId)
    {
        var ids = GetStoredIds();
        ids.Remove(productId);
        SaveIds(ids);

        return Task.CompletedTask;
    }

    public Task ClearAsync()
    {
        SaveIds([]);

        return Task.CompletedTask;
    }

    public Task<int> GetCountAsync()
    {
        return Task.FromResult(GetStoredIds().Count);
    }

    private List<int> GetStoredIds()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        var raw = session?.GetString(SessionKey);

        if (string.IsNullOrWhiteSpace(raw))
        {
            return [];
        }

        return raw.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(value => int.TryParse(value, out var id) ? id : 0)
            .Where(id => id > 0)
            .Distinct()
            .Take(MaxProducts)
            .ToList();
    }

    private void SaveIds(IReadOnlyList<int> ids)
    {
        var session = _httpContextAccessor.HttpContext?.Session;

        if (session is null)
        {
            return;
        }

        session.SetString(SessionKey, string.Join(',', ids));
    }
}
