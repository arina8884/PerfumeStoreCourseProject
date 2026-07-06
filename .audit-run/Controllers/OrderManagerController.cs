using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.MVCC.Constants;
using PerfumeStore.MVCC.Services.Interfaces;

namespace PerfumeStore.MVCC.Controllers;

[Authorize(Roles = UserRoles.OrderManager)]
[Route("order-manager")]
public class OrderManagerController : Controller
{
    private readonly IOrderStatusService _orderStatusService;

    public OrderManagerController(IOrderStatusService orderStatusService)
    {
        _orderStatusService = orderStatusService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var model = await _orderStatusService.GetDashboardAsync();

        return View(model);
    }
}
