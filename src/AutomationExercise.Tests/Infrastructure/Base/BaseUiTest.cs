using AutomationExercise.Tests.Configuration;
using AutomationExercise.Tests.Infrastructure.Browser;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;

namespace AutomationExercise.Tests.Infrastructure.Base;

public abstract class BaseUiTest : BaseTest
{
    protected IBrowser Browser { get; private set; } = null!;
    protected IBrowserContext Context { get; private set; } = null!;
    protected IPage Page { get; private set; } = null!;
    protected AppSettings AppSettings { get; private set; } = null!;

    protected override async Task OnSetUpAsync()
    {
        var factory = Resolve<IBrowserFactory>();
        AppSettings = Resolve<IOptions<AppSettings>>().Value;

        Browser = await factory.CreateBrowserAsync();
        Context = await factory.CreateContextAsync(Browser);
        Page = await factory.CreatePageAsync(Context);
        Page.SetDefaultTimeout(AppSettings.DefaultTimeoutMs);
    }

    protected override async Task OnTearDownAsync(bool testFailed)
    {
        if (testFailed)
        {
            await CaptureScreenshotAsync("failure-screenshot");
        }

        await Page.CloseAsync();
        await Context.CloseAsync();
        await Browser.CloseAsync();
    }

    protected async Task CaptureScreenshotAsync(string name = "screenshot")
    {
        try
        {
            var bytes = await Page.ScreenshotAsync(new PageScreenshotOptions { FullPage = true });
            Logger.AttachScreenshot(bytes, name);
        }
        catch (Exception ex)
        {
            Logger.LogWarning($"Failed to capture screenshot '{name}': {ex.Message}");
        }
    }

    protected async Task NavigateToAsync(string? relativeUrl = null)
    {
        var url = AppSettings.BaseUrl + relativeUrl;
        await Page.GotoAsync(url);
    }
}
