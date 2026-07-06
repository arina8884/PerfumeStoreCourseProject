using Microsoft.AspNetCore.Mvc;
using PerfumeStore.MVCC.Constants;
using PerfumeStore.MVCC.Helpers;
using PerfumeStore.MVCC.Services.Interfaces;
using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.ViewComponents;

public class ClientNavItemsViewComponent : ViewComponent
{
    private readonly ICartService _cartService;
    private readonly IFavoriteService _favoriteService;
    private readonly ICompareService _compareService;

    public ClientNavItemsViewComponent(
        ICartService cartService,
        IFavoriteService favoriteService,
        ICompareService compareService)
    {
        _cartService = cartService;
        _favoriteService = favoriteService;
        _compareService = compareService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        if (!HttpContext.User.IsInRole(UserRoles.Client))
        {
            return Content(string.Empty);
        }

        var userId = HttpContext.User.GetRequiredUserId();
        var model = new ClientNavBadgesViewModel
        {
            CartCount = await _cartService.GetItemsCountAsync(userId),
            FavoritesCount = await _favoriteService.GetCountAsync(userId),
            CompareCount = await _compareService.GetCountAsync()
        };

        return View(model);
    }
}
