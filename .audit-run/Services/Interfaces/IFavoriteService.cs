using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Services.Interfaces;

public interface IFavoriteService
{
    Task<FavoriteListViewModel> GetFavoritesAsync(int userId);

    Task<int> GetCountAsync(int userId);

    Task<bool> IsFavoriteAsync(int userId, int productId);

    Task<bool> AddAsync(int userId, int productId);

    Task<bool> RemoveAsync(int userId, int productId);
}
