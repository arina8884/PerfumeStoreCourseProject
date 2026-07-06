using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.MVCC.Constants;
using PerfumeStore.MVCC.Helpers;
using PerfumeStore.MVCC.Services.Interfaces;
using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Controllers;

[Authorize(Roles = UserRoles.Client)]
public class ReviewsController : Controller
{
    private readonly IReviewService _reviewService;

    public ReviewsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpGet("/client/reviews")]
    public async Task<IActionResult> Index()
    {
        var model = await _reviewService.GetUserReviewsAsync(User.GetRequiredUserId());

        return View(model);
    }

    [HttpGet("/client/reviews/{id:int}/edit")]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await _reviewService.GetReviewForEditAsync(User.GetRequiredUserId(), id);

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost("/client/reviews/{id:int}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ReviewEditViewModel model)
    {
        if (id != model.ReviewId)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var updated = await _reviewService.UpdateReviewAsync(
            User.GetRequiredUserId(),
            model.ReviewId,
            model.Rating,
            model.Comment);

        if (!updated)
        {
            TempData["ErrorMessage"] = "Не удалось обновить отзыв.";
            return RedirectToAction(nameof(Index));
        }

        TempData["SuccessMessage"] = "Отзыв обновлён.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("/client/reviews/{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, string? returnUrl = null)
    {
        var deleted = await _reviewService.DeleteReviewAsync(User.GetRequiredUserId(), id);

        if (!deleted)
        {
            TempData["ErrorMessage"] = "Не удалось удалить отзыв.";
        }
        else
        {
            TempData["SuccessMessage"] = "Отзыв удалён.";
        }

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("/reviews/create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ReviewCreateViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Проверьте правильность заполнения формы отзыва.";

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(Index));
        }

        var created = await _reviewService.CreateReviewAsync(
            User.GetRequiredUserId(),
            model.ProductId,
            model.Rating,
            model.Comment);

        if (!created)
        {
            TempData["ErrorMessage"] = "Не удалось сохранить отзыв. Возможно, отзыв уже существует или товар недоступен.";
        }
        else
        {
            TempData["SuccessMessage"] = "Отзыв опубликован.";
        }

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction(nameof(Index));
    }
}
