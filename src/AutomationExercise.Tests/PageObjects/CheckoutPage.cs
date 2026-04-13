using AutomationExercise.Tests.Infrastructure.Logging;
using AutomationExercise.Tests.PageObjects.Base;
using AutomationExercise.Tests.TestData.Builders;
using Microsoft.Playwright;

namespace AutomationExercise.Tests.PageObjects;

public sealed class CheckoutPage : BasePage
{
    private const string CommentBox       = "textarea.form-control";
    private const string PlaceOrderButton = "a.check_out";
    private const string CardName         = "input[data-qa='name-on-card']";
    private const string CardNumber       = "input[data-qa='card-number']";
    private const string CardCvc          = "input[data-qa='cvc']";
    private const string ExpiryMonth      = "input[data-qa='expiry-month']";
    private const string ExpiryYear       = "input[data-qa='expiry-year']";
    private const string PayButton        = "button[data-qa='pay-button']";
    private const string SuccessMessage   = "p:has-text('Congratulations')";

    public CheckoutPage(IPage page, ITestLogger logger) : base(page, logger) { }

    public override async Task<bool> IsLoadedAsync()
    {
        return await L(PlaceOrderButton).IsVisibleAsync();
    }

    public async Task EnterCommentAndPlaceOrderAsync(string comment = "Test order")
    {
        await FillAsync(CommentBox, comment);
        await ClickAsync(PlaceOrderButton);
    }

    public async Task FillPaymentDetailsAsync(OrderModel order)
    {
        await FillAsync(CardName, order.CardHolder);
        await FillAsync(CardNumber, order.CardNumber);
        await FillAsync(CardCvc, order.Cvv);
        await FillAsync(ExpiryMonth, order.ExpiryMonth);
        await FillAsync(ExpiryYear, order.ExpiryYear);
        await ClickAsync(PayButton);
    }

    public async Task<bool> IsOrderSuccessfulAsync()
    {
        return await L(SuccessMessage).IsVisibleAsync();
    }
}
