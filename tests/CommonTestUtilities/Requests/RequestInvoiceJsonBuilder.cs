using Bogus;
using BarberBoss.Communication.Enums;
using BarberBoss.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestInvoiceJsonBuilder
{
    public static RequestInvoiceJson Build()
    {
        return new Faker<RequestInvoiceJson>()
            .RuleFor(r => r.Title, faker => faker.Commerce.ProductName())
            .RuleFor(r => r.Description, faker => faker.Commerce.ProductDescription())
            .RuleFor(r => r.Date, faker => faker.Date.Past())
            .RuleFor(r => r.PaymentType, faker => faker.PickRandom<PaymentType>())
            .RuleFor(r => r.Value, faker => faker.Random.Float(min: 1, max: 1000))
            .RuleFor(r => r.Tags, faker => faker.Make(1, () => faker.PickRandom<Tag>()));
    }
}
