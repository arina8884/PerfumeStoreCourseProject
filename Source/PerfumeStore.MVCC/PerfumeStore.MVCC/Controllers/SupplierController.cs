using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.MVCC.Constants;
using PerfumeStore.MVCC.Services.Interfaces;
using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Controllers;

[Authorize(Roles = UserRoles.OrderManager)]
[Route("order-manager/suppliers")]
public class SupplierController : Controller
{
    private readonly ISupplierService _supplierService;

    public SupplierController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var model = await _supplierService.GetAllAsync();

        return View(model);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View(new SupplierEditViewModel());
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SupplierEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await _supplierService.CreateAsync(model);
        TempData["SuccessMessage"] = "Поставщик создан.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var model = await _supplierService.GetDetailsAsync(id);

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await _supplierService.GetForEditAsync(id);

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost("{id:int}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SupplierEditViewModel model)
    {
        model.Id = id;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var updated = await _supplierService.UpdateAsync(model);

        if (!updated)
        {
            return NotFound();
        }

        TempData["SuccessMessage"] = "Поставщик обновлён.";

        return RedirectToAction(nameof(Index));
    }
}
