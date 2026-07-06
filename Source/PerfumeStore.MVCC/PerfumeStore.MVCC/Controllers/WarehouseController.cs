using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.MVCC.Constants;
using PerfumeStore.MVCC.Services.Interfaces;

namespace PerfumeStore.MVCC.Controllers;

[Authorize(Roles = UserRoles.OrderManager)]
[Route("order-manager/warehouse")]
public class WarehouseController : Controller
{
    private readonly IInventoryService _inventoryService;

    public WarehouseController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        return View(await _inventoryService.GetWarehouseAsync());
    }
}
