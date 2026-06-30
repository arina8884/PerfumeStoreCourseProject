using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.MVCC.Constants;

namespace PerfumeStore.MVCC.Controllers;

[Authorize(Roles = $"{UserRoles.ContentManager},{UserRoles.Admin}")]
[Route("content-manager")]
public class ContentManagerController : Controller
{
    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }
}
