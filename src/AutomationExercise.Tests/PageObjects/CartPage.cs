using AutomationExercise.Tests.Infrastructure.Logging;
using AutomationExercise.Tests.PageObjects.Base;
using Microsoft.Playwright;

namespace AutomationExercise.Tests.PageObjects;

public sealed class CartPage : BasePage
{
    private const string CartTable       = "#cart_info_table";
    private const string CartRows        = "tr.cart_menu + tr";
    private const string ProductNameCell = ".cart_description h4 a";
    private const string PriceCell       = ".cart_price p";
    private const string TotalPriceCell  = ".cart_total_price";
    private const string CheckoutButton  = "a.check_out";
    private const string EmptyCartMsg    = "#empty_cart";

    public CartPage(IPage page, ITestLogger logger) : base(page, logger) { }

    public override async Task<bool> IsLoadedAsync()
    {
        return await L(CartTable).IsVisibleAsync();
    }

    public async Task<bool> IsCartEmptyAsync()
    {
        return await L(EmptyCartMsg).IsVisibleAsync();
    }

    public async Task<int> GetItemCountAsync()
    {
        // Count by product name links — one per cart row, avoids thead/tbody selector issues
        return await L(ProductNameCell).CountAsync();
    }

    public async Task<string> GetFirstItemNameAsync()
    {
        return await GetTextAsync(ProductNameCell);
    }

    public async Task<decimal> GetTotalPriceAsync()
    {
        var raw = await GetTextAsync(TotalPriceCell);
        var cleaned = raw.Replace("Rs.", string.Empty).Trim();
        return decimal.Parse(cleaned);
    }

    public async Task ProceedToCheckoutAsync()
    {
        Logger.LogStep("Proceed to checkout");
        await ClickAsync(CheckoutButton);
    }
}
