using System.Text;
using Allure.Net.Commons;
using Microsoft.Extensions.Logging;

namespace AutomationExercise.Tests.Infrastructure.Logging;

public sealed class AllureTestLogger : ITestLogger
{
    private readonly ILogger<AllureTestLogger> _logger;

    public AllureTestLogger(ILogger<AllureTestLogger> logger)
    {
        _logger = logger;
    }

    public void LogStep(string description)
    {
        _logger.LogInformation("[STEP] {Description}", description);
        AllureApi.Step(description);
    }

    public void LogInfo(string message)
    {
        _logger.LogInformation("{Message}", message);
    }

    public void LogWarning(string message)
    {
        _logger.LogWarning("{Message}", message);
    }

    public void LogError(string message, Exception? ex = null)
    {
        _logger.LogError(ex, "{Message}", message);
    }

    public void AttachScreenshot(byte[] screenshotBytes, string name = "screenshot")
    {
        AllureApi.AddAttachment(name, "image/png", screenshotBytes);
    }

    public void AttachText(string content, string name, string mimeType = "text/plain")
    {
        AllureApi.AddAttachment(name, mimeType, Encoding.UTF8.GetBytes(content));
    }
}
