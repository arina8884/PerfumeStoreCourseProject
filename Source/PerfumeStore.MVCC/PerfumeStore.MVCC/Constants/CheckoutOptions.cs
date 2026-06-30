namespace PerfumeStore.MVCC.Constants;

public static class CheckoutOptions
{
    public const string DeliveryCourier = "Курьер";
    public const string DeliveryPickup = "Самовывоз из Европочты";
    public const string PaymentOnline = "Картой онлайн";
    public const string PaymentOnDelivery = "При получении";

    public static readonly IReadOnlyList<string> DeliveryMethods =
    [
        DeliveryCourier,
        DeliveryPickup
    ];

    public static readonly IReadOnlyList<string> PaymentMethods =
    [
        PaymentOnline,
        PaymentOnDelivery
    ];
}
