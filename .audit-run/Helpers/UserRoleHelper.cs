using PerfumeStore.MVCC.Constants;

namespace PerfumeStore.MVCC.Helpers;

public static class UserRoleHelper
{
    public static string ToDisplayName(string role) => role switch
    {
        UserRoles.Client => "Клиент",
        UserRoles.OrderManager => "Менеджер заказов",
        UserRoles.ContentManager => "Контент-менеджер",
        UserRoles.Admin => "Администратор",
        _ => role
    };

    public static string ToBadgeClass(string role) => role switch
    {
        UserRoles.Client => "text-bg-primary",
        UserRoles.OrderManager => "text-bg-warning",
        UserRoles.ContentManager => "text-bg-info",
        UserRoles.Admin => "text-bg-dark",
        _ => "text-bg-secondary"
    };
}
