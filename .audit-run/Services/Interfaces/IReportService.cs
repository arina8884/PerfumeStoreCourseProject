using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Services.Interfaces;

public interface IReportService
{
    Task<AdminDashboardViewModel> GetDashboardAsync();

    Task<ReportsViewModel> GetReportsAsync();
}
