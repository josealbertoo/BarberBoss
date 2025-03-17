using BarberBoss.Exception;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Invoicing.Delete;
public class DeleteInvoiceTest : BarberBossClassFixture
{
    private const string METHOD = "api/Invoicing";

    private readonly string _token;
    private readonly long _invoiceId;

    public DeleteInvoiceTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
        _invoiceId = webApplicationFactory.Invoice_MemberTeam.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoDelete(requestUri: $"{METHOD}/{_invoiceId}", token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);

        result = await DoGet(requestUri: $"{METHOD}/{_invoiceId}", token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invoice_Not_Found(string culture)
    {
        var result = await DoDelete(requestUri: $"{METHOD}/1000", token: _token, culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("INVOICE_NOT_FOUND", new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
