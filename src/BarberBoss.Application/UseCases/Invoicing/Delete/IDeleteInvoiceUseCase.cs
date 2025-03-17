namespace BarberBoss.Application.UseCases.Invoicing.Delete;
public interface IDeleteInvoiceUseCase
{
    Task Execute(long id);
}
