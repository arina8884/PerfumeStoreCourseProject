namespace PerfumeStore.MVCC.ViewModels;

public class ManagerOrderHistoryViewModel
{
    public IReadOnlyList<ManagerOrderSummaryViewModel> Orders { get; set; } =
        Array.Empty<ManagerOrderSummaryViewModel>();
}
