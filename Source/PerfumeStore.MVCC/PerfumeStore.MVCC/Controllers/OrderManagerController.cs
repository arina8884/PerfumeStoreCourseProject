using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.MVCC.Constants;

namespace PerfumeStore.MVCC.Controllers;

[Authorize(Roles = $"{UserRoles.OrderManager},{UserRoles.Admin}")]
[Route("order-manager")]
public class OrderManagerController : Controller
{
    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }
}
