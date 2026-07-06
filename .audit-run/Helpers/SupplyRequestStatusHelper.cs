using PerfumeStore.MVCC.Constants;

namespace PerfumeStore.MVCC.Helpers;

public static class SupplyRequestStatusHelper
{
    public static string ToDisplayName(string status) => status switch
    {
        SupplyRequestStatuses.Pending => "Ожидает",
        SupplyRequestStatuses.Approved => "Одобрена",
        SupplyRequestStatuses.Completed => "Выполнена",
        SupplyRequestStatuses.Rejected => "Отклонена",
        _ => status
    };

    public static string ToBadgeClass(string status) => status switch
    {
        SupplyRequestStatuses.Pending => "text-bg-warning",
        SupplyRequestStatuses.Approved => "text-bg-info",
        SupplyRequestStatuses.Completed => "text-bg-success",
        SupplyRequestStatuses.Rejected => "text-bg-danger",
        _ => "text-bg-secondary"
    };
}
