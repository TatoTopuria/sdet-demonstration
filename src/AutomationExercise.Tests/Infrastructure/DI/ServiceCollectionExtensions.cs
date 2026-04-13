using AutomationExercise.Tests.ApiClients;
using AutomationExercise.Tests.Configuration;
using AutomationExercise.Tests.Infrastructure.Browser;
using AutomationExercise.Tests.Infrastructure.Logging;
using AutomationExercise.Tests.Infrastructure.Retry;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AutomationExercise.Tests.Infrastructure.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTestInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<AppSettings>(configuration.GetSection(AppSettings.SectionName));
        services.Configure<BrowserSettings>(configuration.GetSection(BrowserSettings.SectionName));
        services.Configure<ApiSettings>(configuration.GetSection(ApiSettings.SectionName));
        services.Configure<CredentialsSettings>(configuration.GetSection(CredentialsSettings.SectionName));

        services.AddLogging(logging =>
        {
            logging.AddConsole();
            logging.SetMinimumLevel(LogLevel.Debug);
        });

        services.AddSingleton<IBrowserFactory, PlaywrightBrowserFactory>();
        services.AddSingleton<ITestLogger, AllureTestLogger>();
        services.AddSingleton<RetryPolicy>();

        services.AddScoped<ProductApiClient>();
        services.AddScoped<UserApiClient>();
        services.AddScoped<BrandApiClient>();

        return services;
    }

    public static IConfiguration BuildConfiguration()
    {
        var environment = Environment.GetEnvironmentVariable("ENVIRONMENT") ?? "Development";

        return new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("Configuration/appsettings.json", optional: false)
            .AddJsonFile($"Configuration/appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
    }
}
