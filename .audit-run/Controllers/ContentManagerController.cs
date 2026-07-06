using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.MVCC.Constants;
using PerfumeStore.MVCC.Services.Interfaces;
using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Controllers;

[Authorize(Roles = UserRoles.ContentManager)]
[Route("content-manager")]
public class ContentManagerController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public ContentManagerController(
        IProductService productService,
        ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var totalProducts = await _productService.GetTotalCountAsync();
        var archivedProducts = await _productService.GetArchivedCountAsync();

        var model = new ContentManagerDashboardViewModel
        {
            ProductsCount = totalProducts,
            ActiveProductsCount = totalProducts - archivedProducts,
            CategoriesCount = await _categoryService.GetCountAsync(),
            ArchivedProductsCount = archivedProducts,
            RecentProducts = await _productService.GetRecentActiveProductsAsync(5),
            RecentCategories = await _categoryService.GetRecentAsync(5)
        };

        return View(model);
    }
}
