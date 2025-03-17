using BarberBoss.Communication.Requests;

namespace BarberBoss.Application.UseCases.Invoicing.Update;
public interface IUpdateInvoiceUseCase
{
    Task Execute(long id, RequestInvoiceJson request);
}
