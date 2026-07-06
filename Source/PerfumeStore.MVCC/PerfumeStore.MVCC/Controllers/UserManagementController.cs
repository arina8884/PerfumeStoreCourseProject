using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.MVCC.Constants;
using PerfumeStore.MVCC.Services.Interfaces;
using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Controllers;

[Authorize(Roles = UserRoles.Admin)]
[Route("admin/users")]
public class UserManagementController : Controller
{
    private static readonly string[] AllowedRoles =
    [
        UserRoles.Client,
        UserRoles.OrderManager,
        UserRoles.ContentManager,
        UserRoles.Admin
    ];

    private readonly IUserService _userService;

    public UserManagementController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        return View(await _userService.GetAllUsersAsync());
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View(new UserManageViewModel { Role = UserRoles.Client });
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserManageViewModel model)
    {
        if (!IsRoleAllowed(model.Role))
        {
            ModelState.AddModelError(nameof(model.Role), "Недопустимая роль.");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var userId = await _userService.CreateUserAsync(model);
            TempData["SuccessMessage"] = "Пользователь создан.";
            return RedirectToAction(nameof(Details), new { id = userId });
        }
        catch (InvalidOperationException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return View(model);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var model = await _userService.GetUserDetailsAsync(id);

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await _userService.GetUserForManageAsync(id);

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UserManageViewModel model)
    {
        model.Id = id;

        if (!IsRoleAllowed(model.Role))
        {
            ModelState.AddModelError(nameof(model.Role), "Недопустимая роль.");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var updated = await _userService.UpdateUserAsync(model);

            if (!updated)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "Пользователь обновлён.";
            return RedirectToAction(nameof(Details), new { id });
        }
        catch (InvalidOperationException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
            return View(model);
        }
    }

    private static bool IsRoleAllowed(string role) => AllowedRoles.Contains(role);
}
