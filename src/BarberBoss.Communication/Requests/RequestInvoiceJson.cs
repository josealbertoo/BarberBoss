using BarberBoss.Communication.Enums;

namespace BarberBoss.Communication.Requests;
public class RequestInvoiceJson
{
    public string Title { get; set; } = string.Empty;
    public DateTime Date {  get; set; }
    public string Description { get; set; } = string.Empty;
    public float Value { get; set; }
    public PaymentType PaymentType { get; set; }
    public IList<Tag> Tags { get; set; } = [];
}
