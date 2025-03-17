using BarberBoss.Communication.Enums;
using BarberBoss.Exception;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Invoicing.GetById;
public class GetInvoiceByIdTest : BarberBossClassFixture
{
    private const string METHOD = "api/Invoicing";

    private readonly string _token;
    private readonly long _invoiceId;

    public GetInvoiceByIdTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
        _invoiceId = webApplicationFactory.Invoice_MemberTeam.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(requestUri: $"{METHOD}/{_invoiceId}", token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("id").GetInt64().Should().Be(_invoiceId);
        response.RootElement.GetProperty("title").GetString().Should().NotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("description").GetString().Should().NotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("date").GetDateTime().Should().NotBeAfter(DateTime.Today);
        response.RootElement.GetProperty("value").GetDecimal().Should().BeGreaterThan(0);
        response.RootElement.GetProperty("tags").EnumerateArray().Should().NotBeNullOrEmpty();

        var paymentType = response.RootElement.GetProperty("paymentType").GetInt32();
        Enum.IsDefined(typeof(PaymentType), paymentType).Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invoice_Not_Found(string culture)
    {
        var result = await DoGet(requestUri: $"{METHOD}/1000", token: _token, culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("INVOICE_NOT_FOUND", new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
