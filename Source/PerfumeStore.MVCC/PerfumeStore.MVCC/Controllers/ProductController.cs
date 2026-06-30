using Microsoft.AspNetCore.Mvc;
using PerfumeStore.MVCC.Services.Interfaces;

namespace PerfumeStore.MVCC.Controllers;

[Route("product")]
public class ProductController : Controller
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var product = await _productService.GetDetailsAsync(id);

        if (product is null)
        {
            return NotFound();
        }

        return View(product);
    }
}
