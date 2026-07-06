using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.MVCC.Constants;
using PerfumeStore.MVCC.Services.Interfaces;

namespace PerfumeStore.MVCC.Controllers;

[Authorize(Roles = UserRoles.Admin)]
[Route("admin/reports")]
public class ReportsController : Controller
{
    private readonly IReportService _reportService;
    private readonly IReportExportService _reportExportService;

    public ReportsController(
        IReportService reportService,
        IReportExportService reportExportService)
    {
        _reportService = reportService;
        _reportExportService = reportExportService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        return View(await _reportService.GetReportsAsync());
    }

    [HttpGet("export/excel")]
    public async Task<IActionResult> ExportExcel()
    {
        var report = await _reportService.GetReportsAsync();
        var content = _reportExportService.ExportToExcel(report);

        return File(
            content,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"perfume-store-report-{DateTime.Now:yyyyMMdd-HHmm}.xlsx");
    }

    [HttpGet("export/pdf")]
    public async Task<IActionResult> ExportPdf()
    {
        var report = await _reportService.GetReportsAsync();
        var content = _reportExportService.ExportToPdf(report);

        return File(content, "application/pdf", $"perfume-store-report-{DateTime.Now:yyyyMMdd-HHmm}.pdf");
    }

    [HttpGet("export/word")]
    public async Task<IActionResult> ExportWord()
    {
        var report = await _reportService.GetReportsAsync();
        var content = _reportExportService.ExportToWord(report);

        return File(
            content,
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            $"perfume-store-report-{DateTime.Now:yyyyMMdd-HHmm}.docx");
    }
}
