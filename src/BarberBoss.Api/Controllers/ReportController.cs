using BarberBoss.Application.UseCases.Invoicing.Reports.Excel;
using BarberBoss.Application.UseCases.Invoicing.Reports.Pdf;
using BarberBoss.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace BarberBoss.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = Roles.ADMIN)]
public class ReportController : ControllerBase
{
    [HttpGet("excel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetExcel(
        [FromServices] IGenerateInvoicingReportExcelUseCase useCase,
        [FromQuery] DateOnly month)
    {
        byte[] file = await useCase.Execute(month);

        if(file.Length > 0)
            return File(file, MediaTypeNames.Application.Octet, "report.xlsx");

        return NoContent();
    }

    [HttpGet("pdf")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetPdf(
        [FromServices] IGenerateInvoicingReportPdfUseCase useCase,
        [FromQuery] DateOnly month)
    {
        byte[] file = await useCase.Execute(month);

        if (file.Length > 0)
            return File(file, MediaTypeNames.Application.Pdf, "report.pdf");

        return NoContent();
    }
}
