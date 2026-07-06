using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.MVCC.Constants;
using PerfumeStore.MVCC.Models;
using PerfumeStore.MVCC.Services.Interfaces;
using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Controllers;

[Route("account")]
public class AccountController : Controller
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public AccountController(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    [HttpGet("login")]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        return View(new LoginViewModel());
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _authService.ValidateUserAsync(model.Email, model.Password);

        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "Неверный email или пароль.");
            return View(model);
        }

        await SignInUserAsync(user, model.RememberMe);

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToRoleHome(user);
    }

    [HttpGet("register")]
    [AllowAnonymous]
    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    [HttpPost("register")]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (await _userService.EmailExistsAsync(model.Email))
        {
            ModelState.AddModelError(nameof(model.Email), "Пользователь с таким email уже существует.");
            return View(model);
        }

        var userId = await _userService.RegisterClientAsync(
            model.FullName,
            model.Email,
            model.Password,
            model.Phone);

        var user = new User
        {
            Id = userId,
            FullName = model.FullName.Trim(),
            Email = model.Email.Trim(),
            Phone = string.IsNullOrWhiteSpace(model.Phone) ? null : model.Phone.Trim(),
            Role = UserRoles.Client
        };

        await SignInUserAsync(user, isPersistent: false);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost("logout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        HttpContext.Session.Clear();
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet("access-denied")]
    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }

    private IActionResult RedirectToRoleHome(User user)
    {
        return user.Role switch
        {
            UserRoles.OrderManager => Redirect("/order-manager"),
            UserRoles.ContentManager => Redirect("/content-manager"),
            UserRoles.Admin => Redirect("/admin"),
            _ => RedirectToAction("Index", "Home")
        };
    }

    private async Task SignInUserAsync(User user, bool isPersistent)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = isPersistent
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            authProperties);
    }
}
