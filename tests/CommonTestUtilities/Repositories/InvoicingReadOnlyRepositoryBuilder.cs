using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Invoicing;
using Moq;

namespace CommonTestUtilities.Repositories;
public class InvoicingReadOnlyRepositoryBuilder
{
    private readonly Mock<IInvoicingReadOnlyRepository> _repository;

    public InvoicingReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IInvoicingReadOnlyRepository>();
    }

    public InvoicingReadOnlyRepositoryBuilder GetAll(User user, List<Invoice> invoicing)
    {
        _repository.Setup(repository => repository.GetAll(user)).ReturnsAsync(invoicing);

        return this;
    }

    public InvoicingReadOnlyRepositoryBuilder GetById(User user, Invoice? invoice)
    {
        if (invoice is not null)
            _repository.Setup(repository => repository.GetById(user, invoice.Id)).ReturnsAsync(invoice);

        return this;
    }

    public InvoicingReadOnlyRepositoryBuilder FilterByMonth(User user, List<Invoice> invoicing)
    {
        _repository.Setup(repository => repository.FilterByMonth(user, It.IsAny<DateOnly>())).ReturnsAsync(invoicing);

        return this;
    }

    public IInvoicingReadOnlyRepository Build() => _repository.Object;
}
