using BarberBoss.Application.UseCases.Invoicing.Register;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Test.Invoicing.Register;
public class RegisterInvoiceUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestInvoiceJsonBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Title.Should().Be(request.Title);
    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        var loggedUser = UserBuilder.Build();

        var request = RequestInvoiceJsonBuilder.Build();
        request.Title = string.Empty;

        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.TITLE_REQUIRED));
    }

    private RegisterInvoiceUseCase CreateUseCase(BarberBoss.Domain.Entities.User user)
    {
        var repository = InvoicingWriteOnlyRepositoryBuilder.Build();
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new RegisterInvoiceUseCase(repository, unitOfWork, mapper, loggedUser);
    }
}
