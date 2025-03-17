using BarberBoss.Domain.Repositories.Invoicing;
using Moq;

namespace CommonTestUtilities.Repositories;
public class InvoicingWriteOnlyRepositoryBuilder
{
    public static IInvoicingWriteOnlyRepository Build()
    {
        var mock = new Mock<IInvoicingWriteOnlyRepository>();

        return mock.Object;
    }
}
