using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Invoicing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace BarberBoss.Infrastructure.DataAccess.Repositories;
internal class InvoicingRepository : IInvoicingReadOnlyRepository, IInvoicingWriteOnlyRepository, IInvoicingUpdateOnlyRepository
{
    private readonly BarberBossDbContext _dbContext;
    public InvoicingRepository(BarberBossDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Invoice invoice)
    {
        await _dbContext.Invoicing.AddAsync(invoice);
    }

    public async Task Delete(long id)
    {
        var result = await _dbContext.Invoicing.FindAsync(id);

        _dbContext.Invoicing.Remove(result!);
    }

    public async Task<List<Invoice>> GetAll(User user)
    {
        return await _dbContext.Invoicing.AsNoTracking().Where(invoice => invoice.UserId == user.Id).ToListAsync();
    }

    async Task<Invoice?> IInvoicingReadOnlyRepository.GetById(User user, long id)
    {
        return await GetFullInvoice()
            .AsNoTracking()
            .FirstOrDefaultAsync(invoice => invoice.Id == id && invoice.UserId == user.Id);
    }

    async Task<Invoice?> IInvoicingUpdateOnlyRepository.GetById(User user, long id)
    {
        return await GetFullInvoice()
            .FirstOrDefaultAsync(invoice => invoice.Id == id && invoice.UserId == user.Id);
    }

    public void Update(Invoice invoice)
    {
        _dbContext.Invoicing.Update(invoice);
    }

    public async Task<List<Invoice>> FilterByMonth(User user, DateOnly date)
    {
        var startDate = new DateTime(year: date.Year, month: date.Month, day: 1).Date;

        var daysInMonth = DateTime.DaysInMonth(year: date.Year, month: date.Month);
        var endDate = new DateTime(year: date.Year, month: date.Month, day: daysInMonth, hour: 23, minute: 59, second: 59);

        return await _dbContext
            .Invoicing
            .AsNoTracking()
            .Where(invoice => invoice.UserId == user.Id && invoice.Date >= startDate && invoice.Date <= endDate)
            .OrderBy(invoice => invoice.Date)
            .ThenBy(invoice => invoice.Title)
            .ToListAsync();
    }

    private IIncludableQueryable<Invoice, ICollection<Tag>> GetFullInvoice()
    {
        return _dbContext.Invoicing
            .Include(invoice => invoice.Tags);
    }
}
