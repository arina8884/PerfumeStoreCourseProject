using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.MVCC.Constants;

namespace PerfumeStore.MVCC.Controllers;

[Authorize(Roles = UserRoles.Admin)]
[Route("admin")]
public class AdminController : Controller
{
    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }
}
