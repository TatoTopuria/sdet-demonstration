namespace AutomationExercise.Tests.Configuration;

public sealed class AppSettings
{
    public const string SectionName = "App";

    public string BaseUrl { get; init; } = string.Empty;
    public string ApiBaseUrl { get; init; } = string.Empty;
    public int DefaultTimeoutMs { get; init; } = 30_000;
    public string Environment { get; init; } = "Development";
}

public sealed class BrowserSettings
{
    public const string SectionName = "Browser";

    public string Type { get; init; } = "Chromium";
    public bool Headless { get; init; } = true;
    public int SlowMo { get; init; } = 0;
    public int ViewportWidth { get; init; } = 1366;
    public int ViewportHeight { get; init; } = 768;
    public bool RecordVideo { get; init; } = false;
    public string VideoDir { get; init; } = "test-results/videos";
}

public sealed class ApiSettings
{
    public const string SectionName = "Api";

    public int TimeoutSeconds { get; init; } = 30;
    public int MaxRetries { get; init; } = 3;
    public int RetryDelayMs { get; init; } = 500;
}

public sealed class CredentialsSettings
{
    public const string SectionName = "Credentials";

    public string ValidEmail { get; init; } = string.Empty;
    public string ValidPassword { get; init; } = string.Empty;
}
