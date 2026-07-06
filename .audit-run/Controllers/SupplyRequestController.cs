using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.MVCC.Constants;
using PerfumeStore.MVCC.Helpers;
using PerfumeStore.MVCC.Services.Interfaces;
using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Controllers;

[Authorize(Roles = UserRoles.OrderManager)]
[Route("order-manager/supply-requests")]
public class SupplyRequestController : Controller
{
    private readonly ISupplyRequestService _supplyRequestService;

    public SupplyRequestController(ISupplyRequestService supplyRequestService)
    {
        _supplyRequestService = supplyRequestService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var model = await _supplyRequestService.GetAllAsync();

        return View(model);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var model = await _supplyRequestService.GetDetailsAsync(id);

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        return View(await _supplyRequestService.GetCreateModelAsync());
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SupplyRequestEditViewModel model)
    {
        model = await ReloadCreateModelAsync(model);

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var requestId = await _supplyRequestService.CreateAsync(User.GetRequiredUserId(), model);
            TempData["SuccessMessage"] = $"Заявка №{requestId} создана.";
            return RedirectToAction(nameof(Details), new { id = requestId });
        }
        catch (InvalidOperationException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return View(model);
        }
    }

    [HttpPost("{id:int}/approve")]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> Approve(int id) =>
        UpdateStatusAsync(id, SupplyRequestStatuses.Approved);

    [HttpPost("{id:int}/reject")]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> Reject(int id) =>
        UpdateStatusAsync(id, SupplyRequestStatuses.Rejected);

    [HttpPost("{id:int}/complete")]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> Complete(int id) =>
        UpdateStatusAsync(id, SupplyRequestStatuses.Completed);

    private async Task<IActionResult> UpdateStatusAsync(int id, string status)
    {
        var updated = await _supplyRequestService.UpdateStatusAsync(id, status);

        if (!updated)
        {
            TempData["ErrorMessage"] = "Не удалось изменить статус заявки.";
        }
        else
        {
            TempData["SuccessMessage"] = "Статус заявки успешно изменён.";
        }

        return RedirectToAction(nameof(Details), new { id });
    }

    private async Task<SupplyRequestEditViewModel> ReloadCreateModelAsync(SupplyRequestEditViewModel model)
    {
        var template = await _supplyRequestService.GetCreateModelAsync();

        return new SupplyRequestEditViewModel
        {
            SupplierId = model.SupplierId,
            Comment = model.Comment,
            Items = model.Items,
            Suppliers = template.Suppliers,
            Products = template.Products
        };
    }
}
