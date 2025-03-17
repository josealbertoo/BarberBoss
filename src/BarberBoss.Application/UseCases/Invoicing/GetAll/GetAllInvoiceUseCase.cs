using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories.Invoicing;
using BarberBoss.Domain.Services.LoggedUser;

namespace BarberBoss.Application.UseCases.Invoicing.GetAll;
public class GetAllInvoiceUseCase : IGetAllInvoiceUseCase
{
    private readonly IInvoicingReadOnlyRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    public GetAllInvoiceUseCase(IInvoicingReadOnlyRepository repository, IMapper mapper, ILoggedUser loggedUser)
    {
        _repository = repository;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }

    public async Task<ResponseInvoicingJson> Execute()
    {
        var loggedUser = await _loggedUser.Get();

        var result = await _repository.GetAll(loggedUser);

        return new ResponseInvoicingJson
        {
            Invoicing = _mapper.Map<List<ResponseShortInvoiceJson>>(result)
        };
    }
}
