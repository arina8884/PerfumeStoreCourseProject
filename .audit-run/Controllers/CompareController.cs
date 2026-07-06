using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.MVCC.Constants;
using PerfumeStore.MVCC.Services.Interfaces;

namespace PerfumeStore.MVCC.Controllers;

[Authorize(Roles = UserRoles.Client)]
[Route("compare")]
public class CompareController : Controller
{
    private readonly ICompareService _compareService;

    public CompareController(ICompareService compareService)
    {
        _compareService = compareService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        return View(await _compareService.GetCompareAsync());
    }

    [HttpPost("add")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int productId, string? returnUrl = null)
    {
        var added = await _compareService.AddProductAsync(productId);

        if (!added)
        {
            TempData["ErrorMessage"] = "Не удалось добавить товар к сравнению.";
        }
        else
        {
            TempData["SuccessMessage"] = "Товар успешно добавлен к сравнению.";
        }

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("remove")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int productId)
    {
        await _compareService.RemoveProductAsync(productId);
        TempData["SuccessMessage"] = "Товар удалён из сравнения.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("clear")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Clear()
    {
        await _compareService.ClearAsync();
        TempData["SuccessMessage"] = "Список сравнения очищен.";

        return RedirectToAction(nameof(Index));
    }
}
