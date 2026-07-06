using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.MVCC.Constants;
using PerfumeStore.MVCC.Services.Interfaces;
using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Controllers;

[Authorize(Roles = UserRoles.ContentManager)]
[Route("content-manager/products")]
public class ProductManagementController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly IImageUploadService _imageUploadService;

    public ProductManagementController(
        IProductService productService,
        ICategoryService categoryService,
        IImageUploadService imageUploadService)
    {
        _productService = productService;
        _categoryService = categoryService;
        _imageUploadService = imageUploadService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var model = await _productService.GetManagementListAsync();

        return View(model);
    }

    [HttpGet("archived")]
    public async Task<IActionResult> Archived()
    {
        var model = await _productService.GetArchivedProductsAsync();

        return View(model);
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        return View(new ProductEditViewModel
        {
            Categories = await _categoryService.GetAllAsync()
        });
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductEditViewModel model)
    {
        model.Categories = await _categoryService.GetAllAsync();

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            model.StockQuantity = 0;
            var imageUrl = await _imageUploadService.SaveProductImageAsync(model.ImageFile);
            await _productService.CreateAsync(model, imageUrl);
            TempData["SuccessMessage"] = "Товар создан.";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return View(model);
        }
    }

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await _productService.GetForEditAsync(id);

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost("{id:int}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductEditViewModel model)
    {
        model.Id = id;
        model.Categories = await _categoryService.GetAllAsync();

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var existing = await _productService.GetForEditAsync(id);
            if (existing is null)
            {
                return NotFound();
            }

            model.StockQuantity = existing.StockQuantity;
            var imageUrl = await _imageUploadService.SaveProductImageAsync(model.ImageFile);
            var updated = await _productService.UpdateAsync(model, imageUrl);

            if (!updated)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "Товар обновлён.";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return View(model);
        }
    }

    [HttpGet("{id:int}/preview")]
    public async Task<IActionResult> Preview(int id)
    {
        var model = await _productService.GetForPreviewAsync(id);

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost("{id:int}/archive")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Archive(int id)
    {
        var archived = await _productService.ArchiveAsync(id);

        if (!archived)
        {
            TempData["ErrorMessage"] = "Не удалось архивировать товар.";
        }
        else
        {
            TempData["SuccessMessage"] = "Товар перемещён в архив.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/activate")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Activate(int id)
    {
        var activated = await _productService.ActivateAsync(id);

        if (!activated)
        {
            TempData["ErrorMessage"] = "Не удалось восстановить товар.";
        }
        else
        {
            TempData["SuccessMessage"] = "Товар восстановлен.";
        }

        return RedirectToAction(nameof(Archived));
    }
}
