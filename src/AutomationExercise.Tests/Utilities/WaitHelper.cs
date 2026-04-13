using Microsoft.Playwright;

namespace AutomationExercise.Tests.Utilities;

public static class WaitHelper
{
    public static async Task WaitForVisibleAsync(ILocator locator, int timeoutMs = 10_000)
    {
        await locator.WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Visible,
            Timeout = timeoutMs
        });
    }

    public static async Task<bool> WaitUntilAsync(
        Func<Task<bool>> condition,
        int timeoutMs = 10_000,
        int intervalMs = 250,
        string message = "Condition was not met within timeout")
    {
        var deadline = DateTime.UtcNow.AddMilliseconds(timeoutMs);

        while (DateTime.UtcNow < deadline)
        {
            if (await condition())
                return true;

            var remaining = (deadline - DateTime.UtcNow).TotalMilliseconds;
            if (remaining <= 0)
                break;

            await Task.Delay(Math.Min(intervalMs, (int)remaining));
        }

        throw new TimeoutException(message);
    }
}
