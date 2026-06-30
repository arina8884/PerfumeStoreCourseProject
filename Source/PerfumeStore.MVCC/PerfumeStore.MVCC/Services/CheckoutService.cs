using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PerfumeStore.MVCC.Constants;
using PerfumeStore.MVCC.Data;
using PerfumeStore.MVCC.Models;
using PerfumeStore.MVCC.Services.Interfaces;
using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Services;

public class CheckoutService : ICheckoutService
{
    private readonly PerfumeStoreDbContext _context;

    public CheckoutService(PerfumeStoreDbContext context)
    {
        _context = context;
    }

    public async Task<CheckoutViewModel?> GetCheckoutAsync(int userId)
    {
        var items = await BuildCartItemsAsync(userId);

        if (items.Count == 0)
        {
            return null;
        }

        return new CheckoutViewModel
        {
            Items = items
        };
    }

    public async Task<DeliverySelectionViewModel?> GetDeliverySelectionAsync(int userId, string deliveryAddress)
    {
        var items = await BuildCartItemsAsync(userId);

        if (items.Count == 0)
        {
            return null;
        }

        return new DeliverySelectionViewModel
        {
            DeliveryAddress = deliveryAddress.Trim(),
            DeliveryMethods = CheckoutOptions.DeliveryMethods,
            TotalAmount = items.Sum(item => item.TotalPrice)
        };
    }

    public async Task<PaymentSelectionViewModel?> GetPaymentSelectionAsync(
        int userId,
        string deliveryAddress,
        string deliveryMethod)
    {
        if (!IsValidDeliveryMethod(deliveryMethod))
        {
            return null;
        }

        var items = await BuildCartItemsAsync(userId);

        if (items.Count == 0)
        {
            return null;
        }

        return new PaymentSelectionViewModel
        {
            DeliveryAddress = deliveryAddress.Trim(),
            DeliveryMethod = deliveryMethod,
            PaymentMethods = CheckoutOptions.PaymentMethods,
            TotalAmount = items.Sum(item => item.TotalPrice)
        };
    }

    public async Task<CheckoutViewModel?> GetConfirmAsync(
        int userId,
        string deliveryAddress,
        string deliveryMethod,
        string paymentMethod)
    {
        if (!IsValidDeliveryMethod(deliveryMethod) || !IsValidPaymentMethod(paymentMethod))
        {
            return null;
        }

        var items = await BuildCartItemsAsync(userId);

        if (items.Count == 0)
        {
            return null;
        }

        return new CheckoutViewModel
        {
            Items = items,
            DeliveryAddress = deliveryAddress.Trim(),
            DeliveryMethod = deliveryMethod,
            PaymentMethod = paymentMethod
        };
    }

    public async Task<int> PlaceOrderAsync(
        int userId,
        string deliveryAddress,
        string deliveryMethod,
        string paymentMethod)
    {
        if (string.IsNullOrWhiteSpace(deliveryAddress))
        {
            throw new InvalidOperationException("Укажите адрес доставки.");
        }

        if (!IsValidDeliveryMethod(deliveryMethod))
        {
            throw new InvalidOperationException("Выбран недопустимый способ доставки.");
        }

        if (!IsValidPaymentMethod(paymentMethod))
        {
            throw new InvalidOperationException("Выбран недопустимый способ оплаты.");
        }

        await using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var cartItems = await _context.CartItems
                .Where(item => item.Cart.UserId == userId)
                .ToListAsync();

            if (cartItems.Count == 0)
            {
                throw new InvalidOperationException("Корзина пуста.");
            }

            var orderLines = new List<(int ProductId, int Quantity, decimal Price)>();
            decimal totalAmount = 0;

            foreach (var cartItem in cartItems)
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(currentProduct => currentProduct.Id == cartItem.ProductId);

                if (product is null)
                {
                    throw new InvalidOperationException("Один из товаров больше не существует.");
                }

                if (!product.IsActive)
                {
                    throw new InvalidOperationException($"Товар \"{product.Name}\" недоступен для заказа.");
                }

                if (product.StockQuantity <= 0)
                {
                    throw new InvalidOperationException($"Товар \"{product.Name}\" отсутствует на складе.");
                }

                if (cartItem.Quantity <= 0)
                {
                    throw new InvalidOperationException("Недопустимое количество товара в корзине.");
                }

                if (cartItem.Quantity > product.StockQuantity)
                {
                    throw new InvalidOperationException($"Недостаточно остатка для товара \"{product.Name}\".");
                }

                var lineTotal = product.Price * cartItem.Quantity;
                totalAmount += lineTotal;
                orderLines.Add((product.Id, cartItem.Quantity, product.Price));
            }

            var order = new Order
            {
                UserId = userId,
                DeliveryAddress = deliveryAddress.Trim(),
                DeliveryMethod = deliveryMethod,
                PaymentMethod = paymentMethod,
                TotalAmount = totalAmount,
                Status = OrderStatuses.New
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var line in orderLines)
            {
                _context.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = line.ProductId,
                    Quantity = line.Quantity,
                    Price = line.Price
                });

                var product = await _context.Products.FirstAsync(currentProduct => currentProduct.Id == line.ProductId);
                product.StockQuantity -= line.Quantity;
            }

            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return order.Id;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<OrderResultViewModel?> GetOrderResultAsync(int userId, int orderId)
    {
        return await _context.Orders
            .AsNoTracking()
            .Where(order => order.Id == orderId && order.UserId == userId)
            .Select(order => new OrderResultViewModel
            {
                OrderId = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                DeliveryAddress = order.DeliveryAddress,
                DeliveryMethod = order.DeliveryMethod,
                PaymentMethod = order.PaymentMethod,
                Status = order.Status
            })
            .FirstOrDefaultAsync();
    }

    public bool IsValidDeliveryMethod(string deliveryMethod)
    {
        return CheckoutOptions.DeliveryMethods.Contains(deliveryMethod);
    }

    public bool IsValidPaymentMethod(string paymentMethod)
    {
        return CheckoutOptions.PaymentMethods.Contains(paymentMethod);
    }

    private async Task<IReadOnlyList<CartItemViewModel>> BuildCartItemsAsync(int userId)
    {
        return await _context.CartItems
            .AsNoTracking()
            .Where(item => item.Cart.UserId == userId)
            .Where(item => item.Product.IsActive && item.Product.StockQuantity > 0)
            .Where(item => item.Quantity > 0 && item.Quantity <= item.Product.StockQuantity)
            .OrderBy(item => item.Product.Name)
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
            .ToListAsync();
    }
}
