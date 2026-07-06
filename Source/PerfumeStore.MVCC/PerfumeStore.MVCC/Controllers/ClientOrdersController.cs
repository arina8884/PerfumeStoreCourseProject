using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.MVCC.Constants;
using PerfumeStore.MVCC.Helpers;
using PerfumeStore.MVCC.Services.Interfaces;

namespace PerfumeStore.MVCC.Controllers;

[Authorize(Roles = UserRoles.Client)]
[Route("client/orders")]
public class ClientOrdersController : Controller
{
    private readonly IOrderService _orderService;

    public ClientOrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var model = await _orderService.GetUserOrdersAsync(User.GetRequiredUserId());

        return View(model);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var ownerId = await _orderService.GetOrderOwnerIdAsync(id);

        if (ownerId is null)
        {
            return NotFound();
        }

        if (ownerId != User.GetRequiredUserId())
        {
            return Forbid();
        }

        var model = await _orderService.GetOrderDetailsAsync(User.GetRequiredUserId(), id);

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpGet("{id:int}/tracking")]
    public async Task<IActionResult> Tracking(int id)
    {
        var ownerId = await _orderService.GetOrderOwnerIdAsync(id);

        if (ownerId is null)
        {
            return NotFound();
        }

        if (ownerId != User.GetRequiredUserId())
        {
            return Forbid();
        }

        var model = await _orderService.GetOrderTrackingAsync(User.GetRequiredUserId(), id);

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }
}
