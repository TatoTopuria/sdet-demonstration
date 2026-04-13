using Microsoft.Playwright;

namespace AutomationExercise.Tests.Utilities.Extensions;

public static class PlaywrightExtensions
{
    public static async Task FillAndVerifyAsync(this ILocator locator, string value)
    {
        await locator.ClearAsync();
        await locator.FillAsync(value);

        var actual = await locator.InputValueAsync();
        if (actual != value)
            throw new InvalidOperationException(
                $"Fill verification failed. Expected: \"{value}\", Actual: \"{actual}\"");
    }

    public static async Task ClickAndWaitForNavigationAsync(
        this ILocator locator,
        IPage page,
        string? expectedUrlContains = null)
    {
#pragma warning disable CS0612 // WaitForNavigationAsync is intentionally used per spec
        await Task.WhenAll(
            page.WaitForNavigationAsync(),
            locator.ClickAsync()
        );
#pragma warning restore CS0612

        if (expectedUrlContains is not null && !page.Url.Contains(expectedUrlContains))
            throw new InvalidOperationException(
                $"Navigation completed but URL \"{page.Url}\" does not contain \"{expectedUrlContains}\"");
    }
}
