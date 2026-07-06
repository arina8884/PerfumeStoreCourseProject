using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Services.Interfaces;

public interface ICheckoutService
{
    Task<CheckoutViewModel?> GetCheckoutAsync(int userId);

    Task<DeliverySelectionViewModel?> GetDeliverySelectionAsync(int userId, string deliveryAddress);

    Task<PaymentSelectionViewModel?> GetPaymentSelectionAsync(
        int userId,
        string deliveryAddress,
        string deliveryMethod);

    Task<CheckoutViewModel?> GetConfirmAsync(
        int userId,
        string deliveryAddress,
        string deliveryMethod,
        string paymentMethod);

    Task<int> PlaceOrderAsync(
        int userId,
        string deliveryAddress,
        string deliveryMethod,
        string paymentMethod);

    Task<OrderResultViewModel?> GetOrderResultAsync(int userId, int orderId);

    bool IsValidDeliveryMethod(string deliveryMethod);

    bool IsValidPaymentMethod(string paymentMethod);
}
