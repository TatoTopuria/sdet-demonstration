using Microsoft.Playwright;

namespace AutomationExercise.Tests.Utilities;

public static class ScreenshotHelper
{
    public static async Task<byte[]> CaptureFullPageAsync(IPage page)
    {
        return await page.ScreenshotAsync(new PageScreenshotOptions
        {
            FullPage = true
        });
    }

    public static async Task<byte[]> CaptureElementAsync(ILocator locator)
    {
        return await locator.ScreenshotAsync();
    }

    public static async Task SaveToDiskAsync(IPage page, string filePath)
    {
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);

        var bytes = await CaptureFullPageAsync(page);
        await File.WriteAllBytesAsync(filePath, bytes);
    }
}
