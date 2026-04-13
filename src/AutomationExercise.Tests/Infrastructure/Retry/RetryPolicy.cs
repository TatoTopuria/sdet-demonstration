using AutomationExercise.Tests.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AutomationExercise.Tests.Infrastructure.Retry;

public sealed class RetryPolicy
{
    private readonly ApiSettings _settings;
    private readonly ILogger<RetryPolicy> _logger;

    public RetryPolicy(IOptions<ApiSettings> options, ILogger<RetryPolicy> logger)
    {
        _settings = options.Value;
        _logger = logger;
    }

    public async Task<T> ExecuteAsync<T>(
        Func<Task<T>> operation,
        string operationName,
        int? maxRetries = null,
        int? delayMs = null)
    {
        int retries = maxRetries ?? _settings.MaxRetries;
        int delay = delayMs ?? _settings.RetryDelayMs;

        Exception? lastException = null;

        for (int attempt = 1; attempt <= retries + 1; attempt++)
        {
            try
            {
                return await operation();
            }
            catch (Exception ex)
            {
                lastException = ex;

                if (attempt <= retries)
                {
                    _logger.LogWarning(
                        "Attempt {Attempt} for operation '{OperationName}' failed. Retrying...",
                        attempt,
                        operationName);

                    await Task.Delay(delay * attempt);
                }
            }
        }

        throw new InvalidOperationException(
            $"Operation '{operationName}' failed after {retries + 1} attempts.",
            lastException);
    }

    public Task ExecuteAsync(
        Func<Task> operation,
        string operationName,
        int? maxRetries = null,
        int? delayMs = null) =>
        ExecuteAsync<bool>(
            async () => { await operation(); return true; },
            operationName,
            maxRetries,
            delayMs);
}
