using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.MVCC.Constants;
using PerfumeStore.MVCC.Helpers;
using PerfumeStore.MVCC.Services.Interfaces;
using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Controllers;

[Authorize(Roles = UserRoles.OrderManager)]
[Route("order-manager/orders")]
public class OrderManagementController : Controller
{
    private readonly IOrderStatusService _orderStatusService;

    public OrderManagementController(IOrderStatusService orderStatusService)
    {
        _orderStatusService = orderStatusService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var model = await _orderStatusService.GetAllOrdersAsync();

        return View(model);
    }

    [HttpGet("queue")]
    public async Task<IActionResult> Queue()
    {
        var model = await _orderStatusService.GetQueueAsync();

        return View(model);
    }

    [HttpGet("history")]
    public async Task<IActionResult> History()
    {
        var model = await _orderStatusService.GetHistoryAsync();

        return View(model);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var model = await _orderStatusService.GetOrderDetailsAsync(id);

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpGet("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id)
    {
        var model = await _orderStatusService.GetStatusUpdateAsync(id);

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost("{id:int}/status")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, OrderStatusUpdateViewModel model)
    {
        if (id != model.OrderId)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            var current = await _orderStatusService.GetStatusUpdateAsync(id);

            if (current is null)
            {
                return NotFound();
            }

            current.Status = model.Status;
            return View(current);
        }

        var updated = await _orderStatusService.UpdateStatusAsync(id, model.Status);

        if (!updated)
        {
            TempData["ErrorMessage"] = "Не удалось изменить статус заказа.";
            return RedirectToAction(nameof(Details), new { id });
        }

        TempData["SuccessMessage"] = "Статус заказа успешно изменён.";

        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpGet("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        var model = await _orderStatusService.GetCancelAsync(id);

        if (model is null)
        {
            return NotFound();
        }

        if (model.Status == OrderStatuses.Canceled || !OrderStatusHelper.IsCancellationAllowed(model.Status))
        {
            TempData["ErrorMessage"] = "Этот заказ нельзя отменить.";
            return RedirectToAction(nameof(Details), new { id });
        }

        return View(model);
    }

    [HttpPost("{id:int}/cancel")]
    [ValidateAntiForgeryToken]
    [ActionName("Cancel")]
    public async Task<IActionResult> CancelConfirmed(int id)
    {
        var canceled = await _orderStatusService.CancelOrderAsync(id);

        if (!canceled)
        {
            TempData["ErrorMessage"] = "Не удалось отменить заказ.";
            return RedirectToAction(nameof(Details), new { id });
        }

        TempData["SuccessMessage"] = "Заказ отменён.";

        return RedirectToAction(nameof(Details), new { id });
    }
}
