using System.Globalization;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using PerfumeStore.MVCC.Constants;
using PerfumeStore.MVCC.Helpers;
using PerfumeStore.MVCC.Services.Interfaces;
using PerfumeStore.MVCC.ViewModels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PerfumeStore.MVCC.Services;

public class ReportExportService : IReportExportService
{
    private const string ReportTitle = "PerfumeStore — системный отчёт";

    private static readonly string[] OrderStatusOrder =
    [
        OrderStatuses.New,
        OrderStatuses.Confirmed,
        OrderStatuses.Assembling,
        OrderStatuses.InDelivery,
        OrderStatuses.OnTheWay,
        OrderStatuses.Delivered,
        OrderStatuses.Canceled
    ];

    public ReportExportService()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public byte[] ExportToExcel(ReportsViewModel report)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Отчёт");

        worksheet.Cell(1, 1).Value = ReportTitle;
        worksheet.Range(1, 1, 1, 2).Merge();
        worksheet.Range(1, 1, 1, 2).Style.Font.Bold = true;
        worksheet.Range(1, 1, 1, 2).Style.Font.FontSize = 14;
        worksheet.Range(1, 1, 1, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        worksheet.Cell(3, 1).Value = "Показатель";
        worksheet.Cell(3, 2).Value = "Значение";
        worksheet.Range(3, 1, 3, 2).Style.Font.Bold = true;
        worksheet.Range(3, 1, 3, 2).Style.Fill.BackgroundColor = XLColor.LightGray;

        var rows = BuildRows(report);
        var currentRow = 4;

        foreach (var row in rows)
        {
            worksheet.Cell(currentRow, 1).Value = row.Label;
            worksheet.Cell(currentRow, 2).Value = row.Value;

            switch (row.Kind)
            {
                case ReportRowKind.SectionHeader:
                    worksheet.Range(currentRow, 1, currentRow, 2).Style.Font.Bold = true;
                    worksheet.Range(currentRow, 1, currentRow, 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#E8E8E8");
                    break;
                case ReportRowKind.Spacer:
                    worksheet.Row(currentRow).Height = 8;
                    break;
            }

            currentRow++;
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
                page.Header().Text(ReportTitle).FontSize(18).SemiBold();
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

                    var isFirstSection = true;

                    foreach (var row in rows)
                    {
                        if (row.Kind == ReportRowKind.Spacer)
                        {
                            continue;
                        }

                        switch (row.Kind)
                        {
                            case ReportRowKind.SectionHeader:
                                var sectionTopPadding = isFirstSection ? 8f : 16f;
                                isFirstSection = false;

                                table.Cell()
                                    .Background(Colors.Grey.Lighten4)
                                    .PaddingLeft(8)
                                    .PaddingRight(8)
                                    .PaddingBottom(8)
                                    .PaddingTop(sectionTopPadding)
                                    .Text(row.Label)
                                    .SemiBold();
                                table.Cell()
                                    .Background(Colors.Grey.Lighten4)
                                    .PaddingLeft(8)
                                    .PaddingRight(8)
                                    .PaddingBottom(8)
                                    .PaddingTop(sectionTopPadding)
                                    .Text(string.Empty);
                                break;
                            default:
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Text(row.Label);
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(8).Text(row.Value);
                                break;
                        }
                    }
                });
                page.Footer().AlignRight().Text(text =>
                {
                    text.Span("Страница ");
                    text.CurrentPageNumber();
                    text.Span(" из ");
                    text.TotalPages();
                });
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

            body.AppendChild(CreateWordParagraph(ReportTitle, bold: true, fontSize: 28));
            body.AppendChild(CreateWordParagraph(string.Empty));

            var table = new Table(
                new TableProperties(
                    new TableBorders(
                        new TopBorder { Val = BorderValues.Single, Size = 4 },
                        new BottomBorder { Val = BorderValues.Single, Size = 4 },
                        new LeftBorder { Val = BorderValues.Single, Size = 4 },
                        new RightBorder { Val = BorderValues.Single, Size = 4 },
                        new InsideHorizontalBorder { Val = BorderValues.Single, Size = 4 },
                        new InsideVerticalBorder { Val = BorderValues.Single, Size = 4 })));

            table.AppendChild(CreateWordRow("Показатель", "Значение", ReportRowKind.SectionHeader));

            foreach (var row in BuildRows(report))
            {
                table.AppendChild(CreateWordRow(row.Label, row.Value, row.Kind));
            }

            body.AppendChild(table);
            mainPart.Document.Save();
        }

        return stream.ToArray();
    }

    private static Paragraph CreateWordParagraph(string text, bool bold = false, int? fontSize = null)
    {
        var run = new Run(new Text(text) { Space = SpaceProcessingModeValues.Preserve });

        if (bold || fontSize.HasValue)
        {
            run.RunProperties = new RunProperties();

            if (bold)
            {
                run.RunProperties.Bold = new Bold();
            }

            if (fontSize.HasValue)
            {
                run.RunProperties.FontSize = new FontSize { Val = fontSize.Value.ToString(CultureInfo.InvariantCulture) };
            }
        }

        return new Paragraph(run);
    }

    private static TableRow CreateWordRow(string label, string value, ReportRowKind kind)
    {
        var row = new TableRow();
        var isBold = kind is ReportRowKind.SectionHeader;

        if (kind is ReportRowKind.Spacer)
        {
            row.Append(CreateWordSpacerCell());
            row.Append(CreateWordSpacerCell());
            return row;
        }

        row.Append(CreateWordCell(label, isBold));
        row.Append(CreateWordCell(value, isBold));

        return row;
    }

    private static TableCell CreateWordSpacerCell()
    {
        return new TableCell(
            new Paragraph(
                new ParagraphProperties(new SpacingBetweenLines { After = "60", Line = "120", LineRule = LineSpacingRuleValues.Exact }),
                new Run(new Text(string.Empty))));
    }

    private static TableCell CreateWordCell(string text, bool bold, bool italic = false, int colspan = 1)
    {
        var run = new Run(new Text(text) { Space = SpaceProcessingModeValues.Preserve });

        if (bold || italic)
        {
            run.RunProperties = new RunProperties();

            if (bold)
            {
                run.RunProperties.Bold = new Bold();
            }

            if (italic)
            {
                run.RunProperties.Italic = new Italic();
            }
        }

        var cell = new TableCell(new Paragraph(run));

        if (colspan > 1)
        {
            cell.TableCellProperties = new TableCellProperties(
                new GridSpan { Val = colspan },
                new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });
        }

        return cell;
    }

    private static List<ReportRow> BuildRows(ReportsViewModel report)
    {
        var rows = new List<ReportRow>();

        rows.Add(ReportRow.Section("1. Общие сведения"));
        rows.Add(ReportRow.Data("Дата и время формирования отчёта", FormatDateTime(report.GeneratedAt)));
        rows.Add(ReportRow.Data("Количество пользователей", report.UsersCount));
        rows.Add(ReportRow.Data("Количество товаров", report.ProductsCount));
        rows.Add(ReportRow.Data("Количество заказов", report.OrdersCount));
        rows.Add(ReportRow.Data("Количество поставщиков", report.SuppliersCount));
        rows.Add(ReportRow.Data("Количество заявок поставщикам", report.SupplyRequestsCount));
        rows.Add(ReportRow.Data("Количество категорий", report.CategoriesCount));

        rows.Add(ReportRow.Spacer());
        rows.Add(ReportRow.Section("2. Статистика пользователей"));
        rows.Add(ReportRow.Data("Количество клиентов", report.ClientsCount));
        rows.Add(ReportRow.Data("Количество контент-менеджеров", report.ContentManagersCount));
        rows.Add(ReportRow.Data("Количество менеджеров по заказам", report.OrderManagersCount));
        rows.Add(ReportRow.Data("Количество администраторов", report.AdminsCount));

        rows.Add(ReportRow.Spacer());
        rows.Add(ReportRow.Section("3. Статистика товаров"));
        rows.Add(ReportRow.Data("Всего товаров", report.ProductsCount));
        rows.Add(ReportRow.Data("Активных товаров", report.ActiveProductsCount));
        rows.Add(ReportRow.Data("Неактивных товаров", report.InactiveProductsCount));
        rows.Add(ReportRow.Data("Товаров в наличии", report.ProductsInStockCount));
        rows.Add(ReportRow.Data("Товаров без остатка", report.ProductsOutOfStockCount));
        rows.Add(ReportRow.Data("Минимальная цена", FormatPrice(report.MinProductPrice)));
        rows.Add(ReportRow.Data("Максимальная цена", FormatPrice(report.MaxProductPrice)));
        rows.Add(ReportRow.Data("Средняя цена", FormatPrice(report.AverageProductPrice)));

        rows.Add(ReportRow.Spacer());
        rows.Add(ReportRow.Section("4. Статистика заказов"));
        rows.Add(ReportRow.Data("Общее количество заказов", report.OrdersCount));

        foreach (var status in OrderStatusOrder)
        {
            var count = report.OrdersByStatus.TryGetValue(status, out var value) ? value : 0;
            var label = $"{OrderStatusHelper.ToDisplayName(status)} ({status})";
            rows.Add(ReportRow.Data(label, count));
        }

        return rows;
    }

    private static string FormatDateTime(DateTime value)
    {
        return value.ToString("dd.MM.yyyy HH:mm", CultureInfo.GetCultureInfo("ru-RU"));
    }

    private static string FormatPrice(decimal? price)
    {
        return price.HasValue
            ? $"{price.Value.ToString("N2", CultureInfo.GetCultureInfo("ru-RU"))} BYN"
            : "—";
    }

    private enum ReportRowKind
    {
        SectionHeader,
        Data,
        Spacer
    }

    private sealed record ReportRow(string Label, string Value, ReportRowKind Kind)
    {
        public static ReportRow Section(string label) => new(label, string.Empty, ReportRowKind.SectionHeader);

        public static ReportRow Data(string label, int value) => new(label, value.ToString(CultureInfo.InvariantCulture), ReportRowKind.Data);

        public static ReportRow Data(string label, string value) => new(label, value, ReportRowKind.Data);

        public static ReportRow Spacer() => new(string.Empty, string.Empty, ReportRowKind.Spacer);
    }
}
