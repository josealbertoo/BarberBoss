using BarberBoss.Communication.Enums;

namespace BarberBoss.Communication.Responses;
public class ResponseInvoiceJson
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public float Value { get; set; }
    public PaymentType PaymentType { get; set; }
    public IList<Tag> Tags { get; set; } = [];
}
