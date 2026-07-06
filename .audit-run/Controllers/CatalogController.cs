using Microsoft.AspNetCore.Mvc;
using PerfumeStore.MVCC.Services.Interfaces;
using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Controllers;

[Route("catalog")]
public class CatalogController : Controller
{
    private readonly ICatalogService _catalogService;

    public CatalogController(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(ProductFilterViewModel filter)
    {
        var model = await _catalogService.GetCatalogAsync(filter);

        return View(model);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(string? searchQuery, string? sortBy)
    {
        var model = await _catalogService.SearchAsync(searchQuery, sortBy);

        return View("Index", model);
    }

    [HttpGet("category/{id:int}")]
    public async Task<IActionResult> ByCategory(int id, ProductFilterViewModel filter)
    {
        filter.CategoryId = id;
        var model = await _catalogService.GetCatalogAsync(filter);

        return View("Index", model);
    }
}
