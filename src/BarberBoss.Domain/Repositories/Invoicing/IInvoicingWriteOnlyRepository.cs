using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Invoicing;
public interface IInvoicingWriteOnlyRepository
{
    Task Add(Invoice invoice);
    Task Delete(long id);
}
