using Microsoft.Playwright;

namespace AutomationExercise.Tests.Infrastructure.Browser;

public interface IBrowserFactory
{
    Task<IBrowser> CreateBrowserAsync(string? browserType = null);
    Task<IBrowserContext> CreateContextAsync(IBrowser browser);
    Task<IPage> CreatePageAsync(IBrowserContext context);
}
