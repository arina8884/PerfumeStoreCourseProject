namespace PerfumeStore.MVCC.Helpers;

public static class StockStatusHelper
{
    private const int LowStockThreshold = 5;

    public static string GetStatus(int stockQuantity)
    {
        if (stockQuantity <= 0)
        {
            return "out";
        }

        return stockQuantity <= LowStockThreshold ? "low" : "in";
    }

    public static string ToDisplayName(int stockQuantity)
    {
        return GetStatus(stockQuantity) switch
        {
            "in" => "Есть в наличии",
            "low" => "Заканчивается",
            _ => "Нет в наличии"
        };
    }

    public static string ToBadgeClass(int stockQuantity)
    {
        return GetStatus(stockQuantity) switch
        {
            "in" => "stock-badge-in",
            "low" => "stock-badge-low",
            _ => "stock-badge-out"
        };
    }
}
