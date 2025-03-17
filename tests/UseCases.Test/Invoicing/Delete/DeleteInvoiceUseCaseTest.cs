using BarberBoss.Application.UseCases.Invoicing.Delete;
using BarberBoss.Domain.Entities;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Invoicing.Delete;
public class DeleteInvoiceUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var invoice = InvoiceBuilder.Build(loggedUser);

        var useCase = CreateUseCase(loggedUser, invoice);

        var act = async () => await useCase.Execute(invoice.Id);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Invoice_Not_Found()
    {
        var loggedUser = UserBuilder.Build();

        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(id: 1000);

        var result = await act.Should().ThrowAsync<NotFoundException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.INVOICE_NOT_FOUND));
    }

    private DeleteInvoiceUseCase CreateUseCase(User user, Invoice? invoice = null)
    {
        var repositoryWriteOnly = InvoicingWriteOnlyRepositoryBuilder.Build();
        var repository = new InvoicingReadOnlyRepositoryBuilder().GetById(user, invoice).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new DeleteInvoiceUseCase(repositoryWriteOnly, repository, unitOfWork, loggedUser);
    }
}
