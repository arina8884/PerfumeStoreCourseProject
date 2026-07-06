using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.MVCC.Constants;
using PerfumeStore.MVCC.Services.Interfaces;
using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Controllers;

[Authorize(Roles = UserRoles.ContentManager)]
[Route("content-manager/categories")]
public class CategoryManagementController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryManagementController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var categories = await _categoryService.GetAllAsync();

        return View(categories);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View(new CategoryEditViewModel());
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await _categoryService.CreateAsync(model);
        TempData["SuccessMessage"] = "Категория создана.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await _categoryService.GetForEditAsync(id);

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost("{id:int}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CategoryEditViewModel model)
    {
        model.Id = id;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var updated = await _categoryService.UpdateAsync(model);

        if (!updated)
        {
            return NotFound();
        }

        TempData["SuccessMessage"] = "Категория обновлена.";

        return RedirectToAction(nameof(Index));
    }
}
