namespace AutomationExercise.Tests.Infrastructure.Logging;

public interface ITestLogger
{
    void LogStep(string description);
    void LogInfo(string message);
    void LogWarning(string message);
    void LogError(string message, Exception? ex = null);
    void AttachScreenshot(byte[] screenshotBytes, string name = "screenshot");
    void AttachText(string content, string name, string mimeType = "text/plain");
}
