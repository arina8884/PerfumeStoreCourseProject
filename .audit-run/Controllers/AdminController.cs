using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.MVCC.Constants;
using PerfumeStore.MVCC.Services.Interfaces;

namespace PerfumeStore.MVCC.Controllers;

[Authorize(Roles = UserRoles.Admin)]
[Route("admin")]
public class AdminController : Controller
{
    private readonly IReportService _reportService;

    public AdminController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        return View(await _reportService.GetDashboardAsync());
    }
}
