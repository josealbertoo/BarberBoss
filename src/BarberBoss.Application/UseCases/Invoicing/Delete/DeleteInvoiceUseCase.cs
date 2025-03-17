using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Invoicing;
using BarberBoss.Domain.Services.LoggedUser;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Invoicing.Delete;
public class DeleteInvoiceUseCase : IDeleteInvoiceUseCase
{
    private readonly IInvoicingReadOnlyRepository _invoicingReadOnly;
    private readonly IInvoicingWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;

    public DeleteInvoiceUseCase(
        IInvoicingWriteOnlyRepository repository,
        IInvoicingReadOnlyRepository invoicingReadOnly,
        IUnitOfWork unitOfWork,
        ILoggedUser loggedUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
        _invoicingReadOnly = invoicingReadOnly;
    }

    public async Task Execute(long id)
    {
        var loggedUser = await _loggedUser.Get();

        var invoice = await _invoicingReadOnly.GetById(loggedUser, id);
        if (invoice is null)
        {
            throw new NotFoundException(ResourceErrorMessages.INVOICE_NOT_FOUND);
        }

        await _repository.Delete(id);

        await _unitOfWork.Commit();
    }
}
