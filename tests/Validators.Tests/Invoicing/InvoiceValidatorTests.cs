using BarberBoss.Application.UseCases.Invoicing;
using BarberBoss.Communication.Enums;
using BarberBoss.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.Invoicing;
public class InvoiceValidatorTests
{
    [Fact]
    public void Success()
    {
        //Arrange
        var validator = new InvoiceValidator();
        var request = RequestInvoiceJsonBuilder.Build();

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("         ")]
    [InlineData(null)]
    public void Error_Title_Empty(string title)
    {
        //Arrange
        var validator = new InvoiceValidator();
        var request = RequestInvoiceJsonBuilder.Build();
        request.Title = title;

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.TITLE_REQUIRED));
    }

    [Fact]
    public void Error_Date_Future()
    {
        //Arrange
        var validator = new InvoiceValidator();
        var request = RequestInvoiceJsonBuilder.Build();
        request.Date = DateTime.UtcNow.AddDays(1);

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.INVOICING_CANNOT_FOR_THE_FUTURE));
    }

    [Fact]
    public void Error_Payment_Type_Invalid()
    {
        //Arrange
        var validator = new InvoiceValidator();
        var request = RequestInvoiceJsonBuilder.Build();
        request.PaymentType = (PaymentType)700;

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.PAYMENT_TYPE_INVALID));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-2)]
    [InlineData(-7)]
    public void Error_Value_Invalid(float value)
    {
        //Arrange
        var validator = new InvoiceValidator();
        var request = RequestInvoiceJsonBuilder.Build();
        request.Value = value;

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.VALUE_MUST_BE_GREATER_THAN_ZERO));
    }

    [Fact]
    public void Error_Tag_Invalid()
    {
        //Arrange
        var validator = new InvoiceValidator();
        var request = RequestInvoiceJsonBuilder.Build();
        request.Tags.Add((Tag)1000);

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.TAG_TYPE_NOT_SUPPORTED));
    }
}
