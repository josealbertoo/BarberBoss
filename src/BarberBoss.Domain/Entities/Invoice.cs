using BarberBoss.Domain.Enums;

namespace BarberBoss.Domain.Entities;
public class Invoice
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public float Value { get; set; }
    public PaymentType PaymentType { get; set; }
    public ICollection<Tag> Tags { get; set; } = [];

    public long UserId { get; set; }
    public User User { get; set; } = default!;
}
