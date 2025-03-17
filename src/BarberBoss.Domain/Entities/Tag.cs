namespace BarberBoss.Domain.Entities;
public class Tag
{
    public long Id { get; set; }
    public Enums.Tag Value { get; set; }

    public long InvoiceId { get; set; }
    public Invoice Invoice { get; set; } = default!;
}
