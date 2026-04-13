using AutomationExercise.Tests.Infrastructure.Logging;
using Microsoft.Playwright;

namespace AutomationExercise.Tests.PageObjects.Base;

public abstract class BasePage
{
    protected IPage Page { get; }
    protected ITestLogger Logger { get; }

    protected BasePage(IPage page, ITestLogger logger)
    {
        Page = page;
        Logger = logger;
    }

    public abstract Task<bool> IsLoadedAsync();

    protected ILocator L(string selector) => Page.Locator(selector);

    protected async Task ClickAsync(string selector)
    {
        Logger.LogStep($"Click: {selector}");
        await Page.Locator(selector).ClickAsync();
    }

    protected async Task FillAsync(string selector, string value)
    {
        Logger.LogStep($"Fill '{selector}' with '{value}'");
        await Page.Locator(selector).FillAsync(value);
    }

    protected async Task SelectAsync(string selector, string value)
    {
        Logger.LogStep($"Select '{value}' in '{selector}'");
        await Page.Locator(selector).SelectOptionAsync(value);
    }

    protected async Task<string> GetTextAsync(string selector)
    {
        return (await Page.Locator(selector).InnerTextAsync()).Trim();
    }

    protected async Task WaitForUrlAsync(string urlContains)
    {
        await Page.WaitForURLAsync($"**{urlContains}**");
    }
}
