using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.MVCC.Constants;
using PerfumeStore.MVCC.Helpers;
using PerfumeStore.MVCC.Services.Interfaces;

namespace PerfumeStore.MVCC.Controllers;

[Authorize(Roles = UserRoles.Client)]
[Route("cart")]
public class CartController : Controller
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var model = await _cartService.GetCartAsync(User.GetRequiredUserId());

        return View(model);
    }

    [HttpPost("add")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int productId, int quantity = 1, string? returnUrl = null)
    {
        try
        {
            await _cartService.AddAsync(User.GetRequiredUserId(), productId, quantity);
            TempData["SuccessMessage"] = "Товар успешно добавлен в корзину.";
        }
        catch (InvalidOperationException exception)
        {
            TempData["ErrorMessage"] = exception.Message;
        }

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("update")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
    {
        await _cartService.UpdateQuantityAsync(User.GetRequiredUserId(), cartItemId, quantity);
        TempData["SuccessMessage"] = "Количество товара в корзине обновлено.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("remove")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int cartItemId)
    {
        await _cartService.RemoveAsync(User.GetRequiredUserId(), cartItemId);
        TempData["SuccessMessage"] = "Товар удалён из корзины.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("clear")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Clear()
    {
        await _cartService.ClearAsync(User.GetRequiredUserId());
        TempData["SuccessMessage"] = "Корзина очищена.";

        return RedirectToAction(nameof(Index));
    }
}
