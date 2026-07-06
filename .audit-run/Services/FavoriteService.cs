using Microsoft.EntityFrameworkCore;
using PerfumeStore.MVCC.Data;
using PerfumeStore.MVCC.Models;
using PerfumeStore.MVCC.Services.Interfaces;
using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Services;

public class FavoriteService : IFavoriteService
{
    private readonly PerfumeStoreDbContext _context;

    public FavoriteService(PerfumeStoreDbContext context)
    {
        _context = context;
    }

    public async Task<FavoriteListViewModel> GetFavoritesAsync(int userId)
    {
        var items = await _context.Favorites
            .AsNoTracking()
            .Where(favorite => favorite.UserId == userId
                && favorite.Product.IsActive
                && favorite.Product.StockQuantity > 0)
            .OrderBy(favorite => favorite.Product.Name)
            .Select(favorite => new FavoriteItemViewModel
            {
                ProductId = favorite.ProductId,
                Name = favorite.Product.Name,
                Brand = favorite.Product.Brand,
                Price = favorite.Product.Price,
                Volume = favorite.Product.Volume,
                ImageUrl = favorite.Product.ImageUrl,
                CategoryName = favorite.Product.Category.Name
            })
            .ToListAsync();

        return new FavoriteListViewModel
        {
            Items = items
        };
    }

    public async Task<bool> IsFavoriteAsync(int userId, int productId)
    {
        return await _context.Favorites
            .AsNoTracking()
            .AnyAsync(favorite => favorite.UserId == userId && favorite.ProductId == productId);
    }

    public async Task<bool> AddAsync(int userId, int productId)
    {
        var productExists = await _context.Products
            .AsNoTracking()
            .AnyAsync(product => product.Id == productId && product.IsActive);

        if (!productExists)
        {
            return false;
        }

        if (await IsFavoriteAsync(userId, productId))
        {
            return true;
        }

        _context.Favorites.Add(new Favorite
        {
            UserId = userId,
            ProductId = productId
        });

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveAsync(int userId, int productId)
    {
        var favorite = await _context.Favorites
            .FirstOrDefaultAsync(item => item.UserId == userId && item.ProductId == productId);

        if (favorite is null)
        {
            return false;
        }

        _context.Favorites.Remove(favorite);
        await _context.SaveChangesAsync();

        return true;
    }

    public Task<int> GetCountAsync(int userId)
    {
        return _context.Favorites
            .AsNoTracking()
            .CountAsync(favorite => favorite.UserId == userId && favorite.Product.IsActive);
    }
}
