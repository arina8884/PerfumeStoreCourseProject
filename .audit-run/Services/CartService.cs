using Microsoft.EntityFrameworkCore;
using PerfumeStore.MVCC.Data;
using PerfumeStore.MVCC.Models;
using PerfumeStore.MVCC.Services.Interfaces;
using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Services;

public class CartService : ICartService
{
    private readonly PerfumeStoreDbContext _context;

    public CartService(PerfumeStoreDbContext context)
    {
        _context = context;
    }

    public async Task<CartViewModel> GetCartAsync(int userId)
    {
        var cartItems = await _context.CartItems
            .Include(item => item.Product)
            .Where(item => item.Cart.UserId == userId)
            .OrderBy(item => item.Product.Name)
            .ToListAsync();

        var hasChanges = false;

        foreach (var item in cartItems.ToList())
        {
            if (!item.Product.IsActive || item.Product.StockQuantity <= 0)
            {
                _context.CartItems.Remove(item);
                cartItems.Remove(item);
                hasChanges = true;
                continue;
            }

            if (item.Quantity > item.Product.StockQuantity)
            {
                item.Quantity = item.Product.StockQuantity;
                hasChanges = true;
            }
        }

        if (hasChanges)
        {
            await _context.SaveChangesAsync();
        }

        return new CartViewModel
        {
            Items = cartItems
                .Select(item => new CartItemViewModel
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Brand = item.Product.Brand,
                    Volume = item.Product.Volume,
                    Price = item.Product.Price,
                    Quantity = item.Quantity,
                    StockQuantity = item.Product.StockQuantity,
                    ImageUrl = item.Product.ImageUrl
                })
                .ToList()
        };
    }

    public async Task<int> GetItemsCountAsync(int userId)
    {
        return await _context.CartItems
            .AsNoTracking()
            .Where(item => item.Cart.UserId == userId)
            .SumAsync(item => item.Quantity);
    }

    public async Task AddAsync(int userId, int productId, int quantity)
    {
        if (quantity <= 0)
        {
            throw new InvalidOperationException("Количество должно быть больше нуля.");
        }

        var product = await _context.Products
            .FirstOrDefaultAsync(currentProduct => currentProduct.Id == productId && currentProduct.IsActive);

        if (product is null)
        {
            throw new InvalidOperationException("Товар не найден.");
        }

        if (product.StockQuantity <= 0)
        {
            throw new InvalidOperationException("Товара нет в наличии.");
        }

        var cart = await GetOrCreateCartAsync(userId);
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(item => item.CartId == cart.Id && item.ProductId == productId);

        var requestedQuantity = quantity;

        if (cartItem is null)
        {
            cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = productId,
                Quantity = Math.Min(requestedQuantity, product.StockQuantity)
            };

            _context.CartItems.Add(cartItem);
        }
        else
        {
            cartItem.Quantity = Math.Min(cartItem.Quantity + requestedQuantity, product.StockQuantity);
        }

        await _context.SaveChangesAsync();
    }

    public async Task UpdateQuantityAsync(int userId, int cartItemId, int quantity)
    {
        var cartItem = await GetUserCartItemAsync(userId, cartItemId);

        if (cartItem is null)
        {
            return;
        }

        if (quantity <= 0 ||
            !cartItem.Product.IsActive ||
            cartItem.Product.StockQuantity <= 0)
        {
            _context.CartItems.Remove(cartItem);
        }
        else
        {
            cartItem.Quantity = Math.Min(quantity, cartItem.Product.StockQuantity);
        }

        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(int userId, int cartItemId)
    {
        var cartItem = await GetUserCartItemAsync(userId, cartItemId);

        if (cartItem is null)
        {
            return;
        }

        _context.CartItems.Remove(cartItem);
        await _context.SaveChangesAsync();
    }

    public async Task ClearAsync(int userId)
    {
        var cartItems = await _context.CartItems
            .Where(item => item.Cart.UserId == userId)
            .ToListAsync();

        _context.CartItems.RemoveRange(cartItems);
        await _context.SaveChangesAsync();
    }

    private async Task<Cart> GetOrCreateCartAsync(int userId)
    {
        var cart = await _context.Cart.FirstOrDefaultAsync(currentCart => currentCart.UserId == userId);

        if (cart is not null)
        {
            return cart;
        }

        cart = new Cart
        {
            UserId = userId
        };

        _context.Cart.Add(cart);
        await _context.SaveChangesAsync();

        return cart;
    }

    private Task<CartItem?> GetUserCartItemAsync(int userId, int cartItemId)
    {
        return _context.CartItems
            .Include(item => item.Product)
            .FirstOrDefaultAsync(item => item.Id == cartItemId && item.Cart.UserId == userId);
    }
}
