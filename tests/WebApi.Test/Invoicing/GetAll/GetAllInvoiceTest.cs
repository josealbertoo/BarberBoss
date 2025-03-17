using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Invoicing.GetAll;
public class GetAllInvoiceTest : BarberBossClassFixture
{
    private const string METHOD = "api/Invoicing";

    private readonly string _token;

    public GetAllInvoiceTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(requestUri: METHOD, token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("invoicing").EnumerateArray().Should().NotBeNullOrEmpty();
    }
}
