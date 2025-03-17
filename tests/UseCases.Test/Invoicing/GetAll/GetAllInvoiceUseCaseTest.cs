using BarberBoss.Application.UseCases.Invoicing.GetAll;
using BarberBoss.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Invoicing.GetAll;
public class GetAllInvoiceUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var invoicing = InvoiceBuilder.Collection(loggedUser);

        var useCase = CreateUseCase(loggedUser, invoicing);

        var result = await useCase.Execute();

        result.Should().NotBeNull();
        result.Invoicing.Should().NotBeNullOrEmpty().And.AllSatisfy(invoice =>
        {
            invoice.Id.Should().BeGreaterThan(0);
            invoice.Title.Should().NotBeNullOrEmpty();
            invoice.Value.Should().BeGreaterThan(0);
        });
    }

    private GetAllInvoiceUseCase CreateUseCase(User user, List<Invoice> invoicing)
    {
        var repository = new InvoicingReadOnlyRepositoryBuilder().GetAll(user, invoicing).Build();
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GetAllInvoiceUseCase(repository, mapper, loggedUser);
    }
}
