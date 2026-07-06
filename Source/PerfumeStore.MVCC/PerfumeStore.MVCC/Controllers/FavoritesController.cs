using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.MVCC.Constants;
using PerfumeStore.MVCC.Helpers;
using PerfumeStore.MVCC.Services.Interfaces;

namespace PerfumeStore.MVCC.Controllers;

[Authorize(Roles = UserRoles.Client)]
public class FavoritesController : Controller
{
    private readonly IFavoriteService _favoriteService;

    public FavoritesController(IFavoriteService favoriteService)
    {
        _favoriteService = favoriteService;
    }

    [HttpGet("/client/favorites")]
    public async Task<IActionResult> Index()
    {
        var model = await _favoriteService.GetFavoritesAsync(User.GetRequiredUserId());

        return View(model);
    }

    [HttpPost("/favorites/add")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int productId, string? returnUrl = null)
    {
        var added = await _favoriteService.AddAsync(User.GetRequiredUserId(), productId);

        if (!added)
        {
            TempData["ErrorMessage"] = "Не удалось добавить товар в избранное.";
        }
        else
        {
            TempData["SuccessMessage"] = "Товар успешно добавлен в избранное.";
        }

        return RedirectToLocal(returnUrl, nameof(Index), "Favorites");
    }

    [HttpPost("/favorites/remove")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int productId, string? returnUrl = null)
    {
        var removed = await _favoriteService.RemoveAsync(User.GetRequiredUserId(), productId);

        if (!removed)
        {
            TempData["ErrorMessage"] = "Товар не найден в избранном.";
        }
        else
        {
            TempData["SuccessMessage"] = "Товар удалён из избранного.";
        }

        return RedirectToLocal(returnUrl, nameof(Index), "Favorites");
    }

    private IActionResult RedirectToLocal(string? returnUrl, string action, string controller)
    {
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction(action, controller);
    }
}
