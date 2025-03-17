using BarberBoss.Application.UseCases.Invoicing.Delete;
using BarberBoss.Application.UseCases.Invoicing.GetAll;
using BarberBoss.Application.UseCases.Invoicing.GetById;
using BarberBoss.Application.UseCases.Invoicing.Register;
using BarberBoss.Application.UseCases.Invoicing.Update;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberBoss.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class InvoicingController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredInvoiceJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterInvoiceUseCase useCase,
        [FromBody] RequestInvoiceJson request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseInvoicingJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAllInvoicing([FromServices] IGetAllInvoiceUseCase useCase)
    {
        var response = await useCase.Execute();

        if (response.Invoicing.Count != 0)
            return Ok(response);

        return NoContent();
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseInvoiceJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromServices] IGetInvoiceByIdUseCase useCase,
        [FromRoute] long id)
    {
        var response = await useCase.Execute(id);

        return Ok(response);
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromServices] IDeleteInvoiceUseCase useCase,
        [FromRoute] long id)
    {
        await useCase.Execute(id);

        return NoContent();
    }

    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromServices] IUpdateInvoiceUseCase useCase,
        [FromRoute] long id,
        [FromBody] RequestInvoiceJson request)
    {
        await useCase.Execute(id, request);

        return NoContent();
    }
}
