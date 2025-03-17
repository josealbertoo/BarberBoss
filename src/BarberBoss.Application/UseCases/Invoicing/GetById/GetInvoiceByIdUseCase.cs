using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories.Invoicing;
using BarberBoss.Domain.Services.LoggedUser;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Invoicing.GetById;
public class GetInvoiceByIdUseCase : IGetInvoiceByIdUseCase
{
    private readonly IInvoicingReadOnlyRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    public GetInvoiceByIdUseCase(IInvoicingReadOnlyRepository repository, IMapper mapper, ILoggedUser loggedUser)
    {
        _repository = repository;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }

    public async Task<ResponseInvoiceJson> Execute(long id)
    {
        var loggedUser = await _loggedUser.Get();

        var result = await _repository.GetById(loggedUser, id);

        if(result is null)
        {
            throw new NotFoundException(ResourceErrorMessages.INVOICE_NOT_FOUND);
        }

        return _mapper.Map<ResponseInvoiceJson>(result);
    }
}
