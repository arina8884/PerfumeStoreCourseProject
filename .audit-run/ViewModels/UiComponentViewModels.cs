namespace PerfumeStore.MVCC.ViewModels;

public class StatCardViewModel
{
    public string Label { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public string Icon { get; set; } = "bi-bar-chart";

    public string? Hint { get; set; }
}

public class PageHeaderViewModel
{
    public string Title { get; set; } = string.Empty;

    public string? Subtitle { get; set; }

    public string? Icon { get; set; }
}

public class EmptyStateViewModel
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Icon { get; set; } = "bi-inbox";

    public string? ButtonText { get; set; }

    public string? ButtonController { get; set; }

    public string? ButtonAction { get; set; }

    public object? ButtonRouteValues { get; set; }
}

public class ClientActivityItemViewModel
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime OccurredAt { get; set; }

    public string Icon { get; set; } = "bi-clock-history";
}
