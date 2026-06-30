using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Services.Interfaces;

public interface ICartService
{
    Task<CartViewModel> GetCartAsync(int userId);

    Task AddAsync(int userId, int productId, int quantity);

    Task UpdateQuantityAsync(int userId, int cartItemId, int quantity);

    Task RemoveAsync(int userId, int cartItemId);

    Task ClearAsync(int userId);
}
