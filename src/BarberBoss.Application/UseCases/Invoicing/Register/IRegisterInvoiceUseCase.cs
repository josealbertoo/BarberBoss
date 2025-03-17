using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;

namespace BarberBoss.Application.UseCases.Invoicing.Register;
public interface IRegisterInvoiceUseCase
{
    Task<ResponseRegisteredInvoiceJson> Execute(RequestInvoiceJson request);
}
