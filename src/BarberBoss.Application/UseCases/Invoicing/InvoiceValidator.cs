using BarberBoss.Communication.Requests;
using BarberBoss.Exception;
using FluentValidation;

namespace BarberBoss.Application.UseCases.Invoicing;
public class InvoiceValidator : AbstractValidator<RequestInvoiceJson>
{
    public InvoiceValidator()
    {
        RuleFor(invoice => invoice.Title).NotEmpty().WithMessage(ResourceErrorMessages.TITLE_REQUIRED);
        RuleFor(invoice => invoice.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.INVOICING_CANNOT_FOR_THE_FUTURE);
        RuleFor(invoice => invoice.Value).GreaterThan(0).WithMessage(ResourceErrorMessages.VALUE_MUST_BE_GREATER_THAN_ZERO);
        RuleFor(invoice => invoice.PaymentType).IsInEnum().WithMessage(ResourceErrorMessages.PAYMENT_TYPE_INVALID);
        RuleFor(invoice => invoice.Tags).ForEach(rule =>
        {
            rule.IsInEnum().WithMessage(ResourceErrorMessages.TAG_TYPE_NOT_SUPPORTED);
        });
    }
}
