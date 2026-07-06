using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.MVCC.Constants;
using PerfumeStore.MVCC.Helpers;
using PerfumeStore.MVCC.Services.Interfaces;
using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Controllers;

[Authorize(Roles = UserRoles.Client)]
[Route("client")]
public class ClientProfileController : Controller
{
    private readonly IUserService _userService;

    public ClientProfileController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Dashboard()
    {
        var model = await _userService.GetDashboardAsync(User.GetRequiredUserId());

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpGet("profile")]
    public async Task<IActionResult> Index()
    {
        var model = await _userService.GetProfileAsync(User.GetRequiredUserId());

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpGet("profile/edit")]
    public async Task<IActionResult> Edit()
    {
        var model = await _userService.GetEditProfileAsync(User.GetRequiredUserId());

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost("profile/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([Bind(nameof(EditProfileViewModel.FullName), nameof(EditProfileViewModel.Phone))] EditProfileViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var currentProfile = await _userService.GetEditProfileAsync(User.GetRequiredUserId());

            if (currentProfile is not null)
            {
                model.Email = currentProfile.Email;
            }

            return View(model);
        }

        var updated = await _userService.UpdateProfileAsync(
            User.GetRequiredUserId(),
            model.FullName,
            model.Phone);

        if (!updated)
        {
            return NotFound();
        }

        await RefreshUserNameClaimAsync(model.FullName.Trim());

        TempData["SuccessMessage"] = "Профиль успешно обновлён.";

        return RedirectToAction(nameof(Index));
    }

    private async Task RefreshUserNameClaimAsync(string fullName)
    {
        var authResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (!authResult.Succeeded || authResult.Principal is null)
        {
            return;
        }

        var claims = authResult.Principal.Claims
            .Where(claim => claim.Type != ClaimTypes.Name)
            .Append(new Claim(ClaimTypes.Name, fullName))
            .ToList();

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var properties = authResult.Properties ?? new AuthenticationProperties();

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            properties);
    }
}
