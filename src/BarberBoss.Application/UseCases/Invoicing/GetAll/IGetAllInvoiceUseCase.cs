using BarberBoss.Communication.Responses;

namespace BarberBoss.Application.UseCases.Invoicing.GetAll;
public interface IGetAllInvoiceUseCase
{
    Task<ResponseInvoicingJson> Execute();
}
