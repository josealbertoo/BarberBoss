using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Invoicing;
using BarberBoss.Domain.Services.LoggedUser;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Invoicing.Register;
public class RegisterInvoiceUseCase : IRegisterInvoiceUseCase
{
    private readonly IInvoicingWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    public RegisterInvoiceUseCase(
        IInvoicingWriteOnlyRepository repository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggedUser loggedUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }

    public async Task<ResponseRegisteredInvoiceJson> Execute(RequestInvoiceJson request)
    {
        Validate(request);

        var loggedUser = await _loggedUser.Get();

        var invoice = _mapper.Map<Invoice>(request);
        invoice.UserId = loggedUser.Id;

        await _repository.Add(invoice);

        await _unitOfWork.Commit();

        return _mapper.Map<ResponseRegisteredInvoiceJson>(invoice);
    }

    private void Validate(RequestInvoiceJson request)
    {
        var validator = new InvoiceValidator();

        var result = validator.Validate(request);

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
