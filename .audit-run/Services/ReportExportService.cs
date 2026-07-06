using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using PerfumeStore.MVCC.Services.Interfaces;
using PerfumeStore.MVCC.ViewModels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PerfumeStore.MVCC.Services;

public class ReportExportService : IReportExportService
{
    public ReportExportService()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public byte[] ExportToExcel(ReportsViewModel report)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Отчёт");

        worksheet.Cell(1, 1).Value = "Показатель";
        worksheet.Cell(1, 2).Value = "Значение";
        worksheet.Range(1, 1, 1, 2).Style.Font.Bold = true;

        var rows = BuildRows(report);

        for (var index = 0; index < rows.Count; index++)
        {
            worksheet.Cell(index + 2, 1).Value = rows[index].Label;
            worksheet.Cell(index + 2, 2).Value = rows[index].Value;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return stream.ToArray();
    }

    public byte[] ExportToPdf(ReportsViewModel report)
    {
        var rows = BuildRows(report);

        return QuestPDF.Fluent.Document.Create(document =>
        {
            document.Page(page =>
            {
                page.Margin(40);
                page.Header().Text("PerfumeStore — системный отчёт").FontSize(18).SemiBold();
                page.Content().PaddingVertical(20).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(1);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(8).Text("Показатель").SemiBold();
                        header.Cell().Background(Colors.Grey.Lighten3).Padding(8).Text("Значение").SemiBold();
                    });

                    foreach (var row in rows)
                    {
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Text(row.Label);
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Text(row.Value);
                    }
                });
                page.Footer().AlignRight().Text($"Сформировано: {report.GeneratedAt:dd.MM.yyyy HH:mm}");
            });
        }).GeneratePdf();
    }

    public byte[] ExportToWord(ReportsViewModel report)
    {
        using var stream = new MemoryStream();
        using (var document = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document, true))
        {
            var mainPart = document.AddMainDocumentPart();
            mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document(new Body());

            var body = mainPart.Document.Body!;

            body.AppendChild(new Paragraph(new Run(new Text("PerfumeStore — системный отчёт")))
            {
                ParagraphProperties = new ParagraphProperties(new Justification { Val = JustificationValues.Left })
            });

            body.AppendChild(new Paragraph(new Run(new Text($"Сформировано: {report.GeneratedAt:dd.MM.yyyy HH:mm}"))));

            var table = new Table(
                new TableProperties(
                    new TableBorders(
                        new TopBorder { Val = BorderValues.Single, Size = 4 },
                        new BottomBorder { Val = BorderValues.Single, Size = 4 },
                        new LeftBorder { Val = BorderValues.Single, Size = 4 },
                        new RightBorder { Val = BorderValues.Single, Size = 4 },
                        new InsideHorizontalBorder { Val = BorderValues.Single, Size = 4 },
                        new InsideVerticalBorder { Val = BorderValues.Single, Size = 4 })));

            table.AppendChild(CreateWordRow("Показатель", "Значение", true));

            foreach (var row in BuildRows(report))
            {
                table.AppendChild(CreateWordRow(row.Label, row.Value, false));
            }

            body.AppendChild(table);
            mainPart.Document.Save();
        }

        return stream.ToArray();
    }

    private static TableRow CreateWordRow(string label, string value, bool isHeader)
    {
        var row = new TableRow();
        row.Append(CreateWordCell(label, isHeader));
        row.Append(CreateWordCell(value, isHeader));

        return row;
    }

    private static TableCell CreateWordCell(string text, bool isHeader)
    {
        var run = new Run(new Text(text));

        if (isHeader)
        {
            run.RunProperties = new RunProperties(new Bold());
        }

        return new TableCell(new Paragraph(run));
    }

    private static List<(string Label, string Value)> BuildRows(ReportsViewModel report)
    {
        return
        [
            ("Пользователи", report.UsersCount.ToString()),
            ("Заказы", report.OrdersCount.ToString()),
            ("Товары", report.ProductsCount.ToString()),
            ("Поставщики", report.SuppliersCount.ToString()),
            ("Заявки поставщикам", report.SupplyRequestsCount.ToString()),
            ("Категории", report.CategoriesCount.ToString())
        ];
    }
}
