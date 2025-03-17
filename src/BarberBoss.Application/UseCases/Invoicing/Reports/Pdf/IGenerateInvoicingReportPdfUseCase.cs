namespace BarberBoss.Application.UseCases.Invoicing.Reports.Pdf;
public interface IGenerateInvoicingReportPdfUseCase
{
    Task<byte[]> Execute(DateOnly month);
}
