using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Invoicing;
public interface IInvoicingReadOnlyRepository
{
    Task<List<Invoice>> GetAll(Entities.User user);
    Task<Invoice?> GetById(Entities.User user, long id);
    Task<List<Invoice>> FilterByMonth(Entities.User user, DateOnly date);
}
