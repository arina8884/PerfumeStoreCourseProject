using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerfumeStore.MVCC.Constants;
using PerfumeStore.MVCC.Helpers;
using PerfumeStore.MVCC.Services.Interfaces;
using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Controllers;

[Authorize(Roles = UserRoles.Client)]
[Route("checkout")]
public class CheckoutController : Controller
{
    private const string DeliveryAddressKey = "CheckoutDeliveryAddress";
    private const string DeliveryMethodKey = "CheckoutDeliveryMethod";
    private const string PaymentMethodKey = "CheckoutPaymentMethod";

    private readonly ICheckoutService _checkoutService;

    public CheckoutController(ICheckoutService checkoutService)
    {
        _checkoutService = checkoutService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var model = await _checkoutService.GetCheckoutAsync(User.GetRequiredUserId());

        if (model is null)
        {
            return RedirectToAction("Index", "Cart");
        }

        return View(model);
    }

    [HttpPost("")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(CheckoutViewModel model)
    {
        var checkout = await _checkoutService.GetCheckoutAsync(User.GetRequiredUserId());

        if (checkout is null)
        {
            return RedirectToAction("Index", "Cart");
        }

        if (!ModelState.IsValid)
        {
            checkout.DeliveryAddress = model.DeliveryAddress;
            return View(checkout);
        }

        TempData[DeliveryAddressKey] = model.DeliveryAddress.Trim();

        return RedirectToAction(nameof(Delivery));
    }

    [HttpGet("delivery")]
    public async Task<IActionResult> Delivery()
    {
        var deliveryAddress = GetSessionValue(DeliveryAddressKey);

        if (string.IsNullOrWhiteSpace(deliveryAddress))
        {
            return RedirectToAction(nameof(Index));
        }

        var model = await _checkoutService.GetDeliverySelectionAsync(User.GetRequiredUserId(), deliveryAddress);

        if (model is null)
        {
            return RedirectToAction("Index", "Cart");
        }

        KeepCheckoutSession();

        return View(model);
    }

    [HttpPost("delivery")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delivery(DeliverySelectionViewModel model)
    {
        var deliveryAddress = GetSessionValue(DeliveryAddressKey);

        if (string.IsNullOrWhiteSpace(deliveryAddress))
        {
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid || !_checkoutService.IsValidDeliveryMethod(model.SelectedDeliveryMethod))
        {
            ModelState.AddModelError(
                nameof(model.SelectedDeliveryMethod),
                "Выберите корректный способ доставки.");

            var refreshed = await _checkoutService.GetDeliverySelectionAsync(User.GetRequiredUserId(), deliveryAddress);

            if (refreshed is null)
            {
                return RedirectToAction("Index", "Cart");
            }

            refreshed.SelectedDeliveryMethod = model.SelectedDeliveryMethod;
            KeepCheckoutSession();

            return View(refreshed);
        }

        TempData[DeliveryAddressKey] = deliveryAddress;
        TempData[DeliveryMethodKey] = model.SelectedDeliveryMethod;

        return RedirectToAction(nameof(Payment));
    }

    [HttpGet("payment")]
    public async Task<IActionResult> Payment()
    {
        var deliveryAddress = GetSessionValue(DeliveryAddressKey);
        var deliveryMethod = GetSessionValue(DeliveryMethodKey);

        if (string.IsNullOrWhiteSpace(deliveryAddress) || string.IsNullOrWhiteSpace(deliveryMethod))
        {
            return RedirectToAction(nameof(Index));
        }

        var model = await _checkoutService.GetPaymentSelectionAsync(
            User.GetRequiredUserId(),
            deliveryAddress,
            deliveryMethod);

        if (model is null)
        {
            return RedirectToAction("Index", "Cart");
        }

        KeepCheckoutSession();

        return View(model);
    }

    [HttpPost("payment")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Payment(PaymentSelectionViewModel model)
    {
        var deliveryAddress = GetSessionValue(DeliveryAddressKey);
        var deliveryMethod = GetSessionValue(DeliveryMethodKey);

        if (string.IsNullOrWhiteSpace(deliveryAddress) || string.IsNullOrWhiteSpace(deliveryMethod))
        {
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid || !_checkoutService.IsValidPaymentMethod(model.SelectedPaymentMethod))
        {
            ModelState.AddModelError(
                nameof(model.SelectedPaymentMethod),
                "Выберите корректный способ оплаты.");

            var refreshed = await _checkoutService.GetPaymentSelectionAsync(
                User.GetRequiredUserId(),
                deliveryAddress,
                deliveryMethod);

            if (refreshed is null)
            {
                return RedirectToAction("Index", "Cart");
            }

            refreshed.SelectedPaymentMethod = model.SelectedPaymentMethod;
            KeepCheckoutSession();

            return View(refreshed);
        }

        TempData[DeliveryAddressKey] = deliveryAddress;
        TempData[DeliveryMethodKey] = deliveryMethod;
        TempData[PaymentMethodKey] = model.SelectedPaymentMethod;

        return RedirectToAction(nameof(Confirm));
    }

    [HttpGet("confirm")]
    public async Task<IActionResult> Confirm()
    {
        var deliveryAddress = GetSessionValue(DeliveryAddressKey);
        var deliveryMethod = GetSessionValue(DeliveryMethodKey);
        var paymentMethod = GetSessionValue(PaymentMethodKey);

        if (string.IsNullOrWhiteSpace(deliveryAddress) ||
            string.IsNullOrWhiteSpace(deliveryMethod) ||
            string.IsNullOrWhiteSpace(paymentMethod))
        {
            return RedirectToAction(nameof(Index));
        }

        var model = await _checkoutService.GetConfirmAsync(
            User.GetRequiredUserId(),
            deliveryAddress,
            deliveryMethod,
            paymentMethod);

        if (model is null)
        {
            return RedirectToAction("Index", "Cart");
        }

        KeepCheckoutSession();

        return View(model);
    }

    [HttpPost("confirm")]
    [ValidateAntiForgeryToken]
    [ActionName(nameof(Confirm))]
    public async Task<IActionResult> ConfirmSubmit()
    {
        var deliveryAddress = GetSessionValue(DeliveryAddressKey);
        var deliveryMethod = GetSessionValue(DeliveryMethodKey);
        var paymentMethod = GetSessionValue(PaymentMethodKey);

        if (string.IsNullOrWhiteSpace(deliveryAddress) ||
            string.IsNullOrWhiteSpace(deliveryMethod) ||
            string.IsNullOrWhiteSpace(paymentMethod))
        {
            return RedirectToAction(nameof(Index));
        }

        try
        {
            var orderId = await _checkoutService.PlaceOrderAsync(
                User.GetRequiredUserId(),
                deliveryAddress,
                deliveryMethod,
                paymentMethod);

            ClearCheckoutSession();

            TempData["SuccessMessage"] = "Заказ успешно оформлен.";

            return RedirectToAction(nameof(Result), new { orderId });
        }
        catch (InvalidOperationException exception)
        {
            TempData["ErrorMessage"] = exception.Message;
            return RedirectToAction("Index", "Cart");
        }
    }

    [HttpGet("result/{orderId:int}")]
    public async Task<IActionResult> Result(int orderId)
    {
        var model = await _checkoutService.GetOrderResultAsync(User.GetRequiredUserId(), orderId);

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    private string? GetSessionValue(string key)
    {
        return TempData[key]?.ToString();
    }

    private void KeepCheckoutSession()
    {
        TempData.Keep(DeliveryAddressKey);
        TempData.Keep(DeliveryMethodKey);
        TempData.Keep(PaymentMethodKey);
    }

    private void ClearCheckoutSession()
    {
        TempData.Remove(DeliveryAddressKey);
        TempData.Remove(DeliveryMethodKey);
        TempData.Remove(PaymentMethodKey);
    }
}
