using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Net.Mime;

namespace WebApi.Test.Invoicing.Reports;
public class GenerateInvoicingReportTest : BarberBossClassFixture
{
    private const string METHOD = "api/Report";

    private readonly string _adminToken;
    private readonly string _teamMemberToken;
    private readonly DateTime _invoiceDate;

    public GenerateInvoicingReportTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _adminToken = webApplicationFactory.User_Admin.GetToken();
        _teamMemberToken = webApplicationFactory.User_Team_Member.GetToken();
        _invoiceDate = webApplicationFactory.Invoice_Admin.GetDate();
    }

    [Fact]
    public async Task Success_Pdf()
    {
        var result = await DoGet(requestUri: $"{METHOD}/pdf?month={_invoiceDate.ToString("Y", CultureInfo.GetCultureInfo("en-US"))}", token: _adminToken);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Pdf);
    }

    [Fact]
    public async Task Success_Excel()
    {
        var result = await DoGet(requestUri: $"{METHOD}/excel?month={_invoiceDate.ToString("Y", CultureInfo.GetCultureInfo("en-US"))}", token: _adminToken);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Octet);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_Excel()
    {
        var result = await DoGet(requestUri: $"{METHOD}/excel?month={_invoiceDate:Y}", token: _teamMemberToken);

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_Pdf()
    {
        var result = await DoGet(requestUri: $"{METHOD}/pdf?month={_invoiceDate:Y}", token: _teamMemberToken);

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
