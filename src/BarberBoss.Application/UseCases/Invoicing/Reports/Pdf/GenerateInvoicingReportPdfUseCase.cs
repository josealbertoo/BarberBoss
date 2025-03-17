using BarberBoss.Application.UseCases.Invoicing.Reports.Pdf.Colors;
using BarberBoss.Application.UseCases.Invoicing.Reports.Pdf.Fonts;
using BarberBoss.Domain.Extensions;
using BarberBoss.Domain.Reports;
using BarberBoss.Domain.Repositories.Invoicing;
using BarberBoss.Domain.Services.LoggedUser;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using System.Globalization;
using System.Reflection;

namespace BarberBoss.Application.UseCases.Invoicing.Reports.Pdf;
public class GenerateInvoicingReportPdfUseCase : IGenerateInvoicingReportPdfUseCase
{
    private const string CURRENCY_SYMBOL = "R$";
    private const int HEIGHT_ROW_INVOICE_TABLE = 25;

    private readonly IInvoicingReadOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;

    public GenerateInvoicingReportPdfUseCase(IInvoicingReadOnlyRepository repository, ILoggedUser loggedUser)
    {
        _repository = repository;
        _loggedUser = loggedUser;

        GlobalFontSettings.FontResolver = new InvoicingReportFontResolver();
    }

    public async Task<byte[]> Execute(DateOnly month)
    {
        var loggedUser = await _loggedUser.Get();

        var invoicing = await _repository.FilterByMonth(loggedUser, month);
        if (invoicing.Count == 0)
        {
            return [];
        }

        var document = CreateDocument(loggedUser.Name, month);
        var page = CreatePage(document);

        CreateHeaderWithProfilePhotoAndName(loggedUser.Name, page);

        var totalInvoicing = invoicing.Sum(invoice => invoice.Value);
        CreateTotalSpentSection(page, month, totalInvoicing);

        foreach (var invoice in invoicing)
        {
            var table = CreateInvoiceTable(page);

            var row = table.AddRow();
            row.Height = HEIGHT_ROW_INVOICE_TABLE;

            AddInvoiceTitle(row.Cells[0], invoice.Title);
            AddHeaderForValue(row.Cells[3]);

            row = table.AddRow();
            row.Height = HEIGHT_ROW_INVOICE_TABLE;

            row.Cells[0].AddParagraph(invoice.Date.ToString("D"));
            SetStyleBaseForInvoiceInformation(row.Cells[0]);
            row.Cells[0].Format.LeftIndent = 7;

            row.Cells[1].AddParagraph(invoice.Date.ToString("t"));
            SetStyleBaseForInvoiceInformation(row.Cells[1]);

            row.Cells[2].AddParagraph(invoice.PaymentType.PaymentTypeToString());
            SetStyleBaseForInvoiceInformation(row.Cells[2]);

            AddValueForInvoice(row.Cells[3], invoice.Value);

            if(string.IsNullOrWhiteSpace(invoice.Description) == false)
            {
                var descriptionRow = table.AddRow();
                descriptionRow.Height = HEIGHT_ROW_INVOICE_TABLE;

                descriptionRow.Cells[0].AddParagraph(invoice.Description);
                descriptionRow.Cells[0].Format.Font = new Font { Name = FontHelper.ROBOTO_REGULAR, Size = 9, Color = ColorsHelper.GRAY };
                descriptionRow.Cells[0].Shading.Color = ColorsHelper.CYAN_LIGHT_OBS;
                descriptionRow.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                descriptionRow.Cells[0].MergeRight = 2;
                descriptionRow.Cells[0].Format.LeftIndent = 7;

                row.Cells[3].MergeDown = 1;
            }

            AddWhiteSpace(table);
        }

        return RenderDocument(document);
    }

    private Document CreateDocument(string author, DateOnly month)
    {
        var document = new Document();

        document.Info.Title = $"{ResourceReportGenerationMessages.INVOICING_FOR} {month:Y}";
        document.Info.Author = author;

        var style = document.Styles["Normal"];
        style!.Font.Name = FontHelper.BEBASNEUE_REGULAR;

        return document;
    }

    private Section CreatePage(Document document)
    {
        var section = document.AddSection();
        section.PageSetup = document.DefaultPageSetup.Clone();

        section.PageSetup.PageFormat = PageFormat.A4;

        section.PageSetup.LeftMargin = 35;
        section.PageSetup.RightMargin = 35;
        section.PageSetup.TopMargin = 53;
        section.PageSetup.BottomMargin = 53;

        return section;
    }

    private void CreateHeaderWithProfilePhotoAndName(string name, Section page)
    {
        var table = page.AddTable();
        table.AddColumn();
        table.AddColumn("300");

        var row = table.AddRow();

        var assembly = Assembly.GetExecutingAssembly();
        var directoryName = Path.GetDirectoryName(assembly.Location);
        var pathFile = Path.Combine(directoryName!, "Logo", "Logo_Barbearia.png");

        row.Cells[0].AddImage(pathFile);
        row.Cells[1].AddParagraph($"{name}");
        row.Cells[1].Format.LeftIndent = 16;
        row.Cells[1].Format.Font = new Font { Name = FontHelper.BEBASNEUE_REGULAR, Size = 25 };
        row.Cells[1].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;
    }

    private void CreateTotalSpentSection(Section page, DateOnly month, float totalInvoicing)
    {
        var paragraph = page.AddParagraph();
        paragraph.Format.SpaceBefore = "35";
        paragraph.Format.SpaceAfter = "35";

        var title = string.Format(ResourceReportGenerationMessages.TOTAL_SPENT_IN, month.ToString("Y"));

        paragraph.AddFormattedText(title, new Font { Name = FontHelper.ROBOTO_MEDIUM, Size = 15 });

        paragraph.AddLineBreak();

        paragraph.AddFormattedText(@String.Format(new CultureInfo("pt-BR"), "{0:C}", totalInvoicing), new Font { Name = FontHelper.BEBASNEUE_REGULAR, Size = 50 }); //$"{CURRENCY_SYMBOL} {totalInvoicing:f2} "
    }

    private Table CreateInvoiceTable(Section page)
    {
        var table = page.AddTable();

        table.AddColumn("265").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("70").Format.Alignment = ParagraphAlignment.Right;
        return table;
    }

    private void AddInvoiceTitle(Cell cell, string invoiceTitle)
    {
        cell.AddParagraph(invoiceTitle);
        cell.Format.Font = new Font { Name = FontHelper.BEBASNEUE_REGULAR, Size = 15, Color = ColorsHelper.WHITE };
        cell.Shading.Color = ColorsHelper.CYAN_DARK_TITLE;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.MergeRight = 2;
        cell.Format.LeftIndent = 7;
    }

    private void AddHeaderForValue(Cell cell)
    {
        cell.AddParagraph(ResourceReportGenerationMessages.VALUE);
        cell.Format.Font = new Font { Name = FontHelper.BEBASNEUE_REGULAR, Size = 15, Color = ColorsHelper.WHITE };
        cell.Shading.Color = ColorsHelper.CYAN_DARK_VALUE;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void SetStyleBaseForInvoiceInformation(Cell cell)
    {
        cell.Format.Font = new Font { Name = FontHelper.ROBOTO_REGULAR, Size = 10, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.CYAN_LIGHT_BODY;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void AddValueForInvoice(Cell cell, float value)
    {
        cell.AddParagraph(@String.Format(new CultureInfo("pt-BR"), "{0:C}", value));//$"{CURRENCY_SYMBOL} {value:f2}");
        cell.Format.Font = new Font { Name = FontHelper.ROBOTO_REGULAR, Size = 10, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.WHITE;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void AddWhiteSpace(Table table)
    {
        var row = table.AddRow();
        row.Height = 30;
        row.Borders.Visible = false;
    }

    private byte[] RenderDocument(Document document)
    {
        var renderer = new PdfDocumentRenderer
        {
            Document = document,
        };

        renderer.RenderDocument();

        using var file = new MemoryStream();
        renderer.PdfDocument.Save(file);

        return file.ToArray();
    }
}