using PerfumeStore.MVCC.ViewModels;

namespace PerfumeStore.MVCC.Services.Interfaces;

public interface IReportExportService
{
    byte[] ExportToExcel(ReportsViewModel report);

    byte[] ExportToPdf(ReportsViewModel report);

    byte[] ExportToWord(ReportsViewModel report);
}
