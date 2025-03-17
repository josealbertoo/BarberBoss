using Bogus;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Enums;

namespace CommonTestUtilities.Entities;
public class InvoiceBuilder
{
    public static List<Invoice> Collection(User user, uint count = 2)
    {
        var list = new List<Invoice>();

        if (count == 0)
            count = 1;

        var invoiceId = 1;

        for (int i = 0; i < count; i++)
        {
            var invoice = Build(user);
            invoice.Id = invoiceId++;

            list.Add(invoice);
        }

        return list;
    }

    public static Invoice Build(User user)
    {
        return new Faker<Invoice>()
            .RuleFor(u => u.Id, _ => 1)
            .RuleFor(u => u.Title, faker => faker.Commerce.ProductName())
            .RuleFor(r => r.Description, faker => faker.Commerce.ProductDescription())
            .RuleFor(r => r.Date, faker => faker.Date.Past())
            .RuleFor(r => r.Value, faker => faker.Random.Float(min: 1, max: 1000))
            .RuleFor(r => r.PaymentType, faker => faker.PickRandom<PaymentType>())
            .RuleFor(r => r.UserId, _ => user.Id)
            .RuleFor(r => r.Tags, faker => faker.Make(1, () => new BarberBoss.Domain.Entities.Tag
            {
                Id = 1,
                Value = faker.PickRandom<BarberBoss.Domain.Enums.Tag>(),
                InvoiceId = 1
            }));
    }
}
