using BarberBoss.Domain.Extensions;
using BarberBoss.Domain.Reports;
using BarberBoss.Domain.Repositories.Invoicing;
using BarberBoss.Domain.Services.LoggedUser;
using ClosedXML.Excel;

namespace BarberBoss.Application.UseCases.Invoicing.Reports.Excel;
public class GenerateInvoicingReportExcelUseCase : IGenerateInvoicingReportExcelUseCase
{
    private const string CURRENCY_SYMBOL = "R$";
    private readonly IInvoicingReadOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;

    public GenerateInvoicingReportExcelUseCase(IInvoicingReadOnlyRepository repository, ILoggedUser loggedUser)
    {
        _repository = repository;
        _loggedUser = loggedUser;
    }

    public async Task<byte[]> Execute(DateOnly month)
    {
        var loggedUser = await _loggedUser.Get();

        var invoicing = await _repository.FilterByMonth(loggedUser, month);
        if(invoicing.Count == 0)
        {
            return [];
        }

        using var workbook = new XLWorkbook();

        workbook.Author = loggedUser.Name;
        workbook.Style.Font.FontSize = 12;
        workbook.Style.Font.FontName = "Times New Roman";

        var worksheet = workbook.Worksheets.Add(month.ToString("Y"));

        InsertHeader(worksheet);

        var raw = 2;
        foreach(var invoice in invoicing)
        {
            worksheet.Cell($"A{raw}").Value = invoice.Title;
            worksheet.Cell($"B{raw}").Value = invoice.Date;
            worksheet.Cell($"C{raw}").Value = invoice.PaymentType.PaymentTypeToString();
            
            worksheet.Cell($"D{raw}").Value = invoice.Value;
            worksheet.Cell($"D{raw}").Style.NumberFormat.Format = $"{CURRENCY_SYMBOL} #,##0.00"; //###,#0.00";
            
            worksheet.Cell($"E{raw}").Value = invoice.Description;

            raw++;
        }

        worksheet.Columns().AdjustToContents();

        var file = new MemoryStream();
        workbook.SaveAs(file);

        return file.ToArray();
    }

    private void InsertHeader(IXLWorksheet worksheet)
    {
        worksheet.Cell("A1").Value = ResourceReportGenerationMessages.TITLE;
        worksheet.Cell("B1").Value = ResourceReportGenerationMessages.DATE;
        worksheet.Cell("C1").Value = ResourceReportGenerationMessages.PAYMENT_TYPE;
        worksheet.Cell("D1").Value = ResourceReportGenerationMessages.VALUE;
        worksheet.Cell("E1").Value = ResourceReportGenerationMessages.DESCRIPTION;

        worksheet.Cells("A1:E1").Style.Font.Bold = true;
        worksheet.Cells("A1:E1").Style.Font.FontColor = XLColor.White;
        worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#205858");

        worksheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        worksheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    }
}