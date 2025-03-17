using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Invoicing;
public interface IInvoicingUpdateOnlyRepository
{
    Task<Invoice?> GetById(Entities.User user, long id);
    void Update(Invoice invoice);
}
