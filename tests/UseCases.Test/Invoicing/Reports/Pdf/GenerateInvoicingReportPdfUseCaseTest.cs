using BarberBoss.Application.UseCases.Invoicing.Reports.Pdf;
using BarberBoss.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Invoicing.Reports.Pdf;
public class GenerateInvoicingReportPdfUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var Invoicing = InvoiceBuilder.Collection(loggedUser);

        var useCase = CreateUseCase(loggedUser, Invoicing);

        var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today));

        result.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Success_Empty()
    {
        var loggedUser = UserBuilder.Build();

        var useCase = CreateUseCase(loggedUser, []);

        var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today));

        result.Should().BeEmpty();
    }

    private GenerateInvoicingReportPdfUseCase CreateUseCase(User user, List<Invoice> Invoicing)
    {
        var repository = new InvoicingReadOnlyRepositoryBuilder().FilterByMonth(user, Invoicing).Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GenerateInvoicingReportPdfUseCase(repository, loggedUser);
    }
}
