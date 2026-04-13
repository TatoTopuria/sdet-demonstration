using AutomationExercise.Tests.Infrastructure.Logging;
using AutomationExercise.Tests.PageObjects.Base;
using Microsoft.Playwright;

namespace AutomationExercise.Tests.PageObjects.Components;

public sealed class NavbarComponent : BasePage, IPageComponent
{
    private const string HomeLink = "a[href='/']";
    private const string ProductsLink = "a[href='/products']";
    private const string CartLink = "a[href='/view_cart']";
    private const string LoginLink = "a[href='/login']";
    private const string LoggedInUser = "a:has-text('Logged in as')";
    private const string LogoutLink = "a[href='/logout']";

    public NavbarComponent(IPage page, ITestLogger logger) : base(page, logger)
    {
    }

    public override async Task<bool> IsLoadedAsync()
    {
        return await L(HomeLink).IsVisibleAsync();
    }

    public async Task<bool> IsUserLoggedInAsync()
    {
        try
        {
            await L(LoggedInUser).WaitForAsync(new LocatorWaitForOptions { Timeout = 8000 });
            return true;
        }
        catch (TimeoutException)
        {
            return false;
        }
    }

    public async Task<string> GetLoggedInUsernameAsync()
    {
        var text = await GetTextAsync(LoggedInUser);
        return text.Replace("Logged in as", string.Empty).Trim();
    }

    public async Task GoToProductsAsync()
    {
        await ClickAsync(ProductsLink);
    }

    public async Task GoToCartAsync()
    {
        await ClickAsync(CartLink);
    }

    public async Task GoToLoginAsync()
    {
        await ClickAsync(LoginLink);
    }

    public async Task LogoutAsync()
    {
        await ClickAsync(LogoutLink);
        await WaitForUrlAsync("/login");
    }
}
