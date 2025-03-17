using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Invoicing;
using Moq;

namespace CommonTestUtilities.Repositories;
public class InvoicingUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IInvoicingUpdateOnlyRepository> _repository;

    public InvoicingUpdateOnlyRepositoryBuilder()
    {
        _repository = new Mock<IInvoicingUpdateOnlyRepository>();
    }

    public InvoicingUpdateOnlyRepositoryBuilder GetById(User user, Invoice? invoice)
    {
        if (invoice is not null)
            _repository.Setup(repository => repository.GetById(user, invoice.Id)).ReturnsAsync(invoice);

        return this;
    }

    public IInvoicingUpdateOnlyRepository Build() => _repository.Object;
}
