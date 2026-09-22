using AutomationExercise.Tests.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;

namespace AutomationExercise.Tests.Infrastructure.Browser;

public sealed class PlaywrightBrowserFactory : IBrowserFactory
{
    private readonly BrowserSettings _settings;
    private IPlaywright? _playwright;

    public PlaywrightBrowserFactory(IOptions<BrowserSettings> options)
    {
        _settings = options.Value;
    }

    public async Task<IBrowser> CreateBrowserAsync(string? browserType = null)
    {
        _playwright ??= await Playwright.CreateAsync();

        var type = (browserType ?? _settings.Type).ToLowerInvariant();
        var launchOptions = new BrowserTypeLaunchOptions
        {
            Headless = _settings.Headless,
            SlowMo = _settings.SlowMo,
        };

        return type switch
        {
            "firefox" => await _playwright.Firefox.LaunchAsync(launchOptions),
            "webkit"  => await _playwright.Webkit.LaunchAsync(launchOptions),
            _         => await _playwright.Chromium.LaunchAsync(launchOptions),
        };
    }

    public async Task<IBrowserContext> CreateContextAsync(IBrowser browser)
    {
        var contextOptions = new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize
            {
                Width  = _settings.ViewportWidth,
                Height = _settings.ViewportHeight,
            },
        };

        if (_settings.RecordVideo)
        {
            Directory.CreateDirectory(_settings.VideoDir);
            contextOptions.RecordVideoDir = _settings.VideoDir;
        }

        return await browser.NewContextAsync(contextOptions);
    }

    public async Task<IPage> CreatePageAsync(IBrowserContext context)
    {
        var page = await context.NewPageAsync();
        await page.RouteAsync("**/*googlesyndication*", async route => { try { await route.AbortAsync(); } catch { } });
        await page.RouteAsync("**/*googleads*", async route => { try { await route.AbortAsync(); } catch { } });
        await page.RouteAsync("**/*doubleclick*", async route => { try { await route.AbortAsync(); } catch { } });
        await page.RouteAsync("**/*adservice.google*", async route => { try { await route.AbortAsync(); } catch { } });
        return page;
    }
}
