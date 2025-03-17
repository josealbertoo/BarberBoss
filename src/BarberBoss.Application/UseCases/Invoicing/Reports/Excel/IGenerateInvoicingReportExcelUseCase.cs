namespace BarberBoss.Application.UseCases.Invoicing.Reports.Excel;
public interface IGenerateInvoicingReportExcelUseCase
{
    Task<byte[]> Execute(DateOnly month);
}
