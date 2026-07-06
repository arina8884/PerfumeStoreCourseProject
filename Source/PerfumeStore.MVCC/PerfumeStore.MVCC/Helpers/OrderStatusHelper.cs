using PerfumeStore.MVCC.Constants;

namespace PerfumeStore.MVCC.Helpers;

public static class OrderStatusHelper
{
    private static readonly IReadOnlyDictionary<string, string> DisplayNames =
        new Dictionary<string, string>
        {
            [OrderStatuses.New] = "Новый",
            [OrderStatuses.Confirmed] = "Подтверждён",
            [OrderStatuses.Assembling] = "Комплектуется",
            [OrderStatuses.InDelivery] = "Передан в доставку",
            [OrderStatuses.OnTheWay] = "В пути",
            [OrderStatuses.Delivered] = "Доставлен",
            [OrderStatuses.Canceled] = "Отменён"
        };

    private static readonly string[] CancellableStatuses =
    {
        OrderStatuses.New,
        OrderStatuses.Confirmed,
        OrderStatuses.Assembling,
        OrderStatuses.InDelivery,
        OrderStatuses.OnTheWay
    };

    private static readonly IReadOnlyDictionary<string, string[]> AllowedTransitions =
        new Dictionary<string, string[]>
        {
            [OrderStatuses.New] = [OrderStatuses.Confirmed, OrderStatuses.Canceled],
            [OrderStatuses.Confirmed] = [OrderStatuses.Assembling, OrderStatuses.Canceled],
            [OrderStatuses.Assembling] = [OrderStatuses.InDelivery, OrderStatuses.Canceled],
            [OrderStatuses.InDelivery] = [OrderStatuses.OnTheWay, OrderStatuses.Canceled],
            [OrderStatuses.OnTheWay] = [OrderStatuses.Delivered, OrderStatuses.Canceled],
            [OrderStatuses.Delivered] = [],
            [OrderStatuses.Canceled] = []
        };

    public static string ToDisplayName(string status)
    {
        return DisplayNames.TryGetValue(status, out var displayName)
            ? displayName
            : status;
    }

    public static string ToBadgeClass(string status)
    {
        return status switch
        {
            OrderStatuses.New => "text-bg-primary",
            OrderStatuses.Confirmed => "text-bg-info",
            OrderStatuses.Assembling => "text-bg-warning",
            OrderStatuses.InDelivery => "text-bg-secondary",
            OrderStatuses.OnTheWay => "text-bg-dark",
            OrderStatuses.Delivered => "text-bg-success",
            OrderStatuses.Canceled => "text-bg-danger",
            _ => "text-bg-secondary"
        };
    }

    public static bool IsCancellationAllowed(string status)
    {
        return CancellableStatuses.Contains(status);
    }

    public static bool IsTransitionAllowed(string currentStatus, string newStatus)
    {
        return AllowedTransitions.TryGetValue(currentStatus, out var transitions)
            && transitions.Contains(newStatus);
    }

    public static IReadOnlyList<string> GetAllowedTransitions(string currentStatus)
    {
        return AllowedTransitions.TryGetValue(currentStatus, out var transitions)
            ? transitions
            : Array.Empty<string>();
    }
}
