using AutomationExercise.Tests.Infrastructure.Logging;
using AutomationExercise.Tests.PageObjects.Base;
using AutomationExercise.Tests.PageObjects.Components;
using Microsoft.Playwright;

namespace AutomationExercise.Tests.PageObjects;

public sealed class LoginPage : BasePage
{
    private const string LoginEmailInput    = "input[data-qa='login-email']";
    private const string LoginPasswordInput = "input[data-qa='login-password']";
    private const string LoginButton        = "button[data-qa='login-button']";
    private const string LoginErrorMessage  = "p:has-text('Your email or password is incorrect')";
    private const string RegisterNameInput  = "input[data-qa='signup-name']";
    private const string RegisterEmailInput = "input[data-qa='signup-email']";
    private const string RegisterButton     = "button[data-qa='signup-button']";
    private const string RegisterError      = "p:has-text('Email Address already exist')";

    public NavbarComponent Navbar { get; }

    public LoginPage(IPage page, ITestLogger logger) : base(page, logger)
    {
        Navbar = new NavbarComponent(page, logger);
    }

    public override async Task<bool> IsLoadedAsync()
    {
        return await L(LoginButton).IsVisibleAsync();
    }

    public async Task LoginAsync(string email, string password)
    {
        await FillAsync(LoginEmailInput, email);
        await FillAsync(LoginPasswordInput, password);
        await ClickAsync(LoginButton);
        await Page.WaitForLoadStateAsync(Microsoft.Playwright.LoadState.NetworkIdle);
    }

    public async Task<bool> IsLoginErrorVisibleAsync()
    {
        return await L(LoginErrorMessage).IsVisibleAsync();
    }

    public async Task SignUpAsync(string name, string email)
    {
        await FillAsync(RegisterNameInput, name);
        await FillAsync(RegisterEmailInput, email);
        await ClickAsync(RegisterButton);
        await WaitForUrlAsync("/signup");
    }

    public async Task<bool> IsRegisterEmailErrorVisibleAsync()
    {
        return await L(RegisterError).IsVisibleAsync();
    }
}
