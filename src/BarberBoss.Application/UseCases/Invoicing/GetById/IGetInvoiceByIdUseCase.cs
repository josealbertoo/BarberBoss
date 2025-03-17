using BarberBoss.Communication.Responses;

namespace BarberBoss.Application.UseCases.Invoicing.GetById;
public interface IGetInvoiceByIdUseCase
{
    Task<ResponseInvoiceJson> Execute(long id);
}
