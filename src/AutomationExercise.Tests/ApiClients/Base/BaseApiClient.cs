using AutomationExercise.Tests.Configuration;
using AutomationExercise.Tests.Infrastructure.Retry;
using AutomationExercise.Tests.Utilities;
using Microsoft.Extensions.Options;
using RestSharp;

namespace AutomationExercise.Tests.ApiClients.Base;

public abstract class BaseApiClient
{
    protected readonly RestClient Client;
    protected readonly RetryPolicy RetryPolicy;

    protected BaseApiClient(
        IOptions<AppSettings> appSettings,
        IOptions<ApiSettings> apiSettings,
        RetryPolicy retryPolicy)
    {
        RetryPolicy = retryPolicy;

        var options = new RestClientOptions(appSettings.Value.ApiBaseUrl)
        {
            Timeout = TimeSpan.FromSeconds(apiSettings.Value.TimeoutSeconds)
        };

        Client = new RestClient(options);
    }

    protected async Task<RestResponse> ExecuteAsync(RestRequest request, string operationName)
    {
        return await RetryPolicy.ExecuteAsync(() => Client.ExecuteAsync(request), operationName);
    }

    protected async Task<T?> ExecuteAndDeserializeAsync<T>(RestRequest request, string operationName)
    {
        var response = await ExecuteAsync(request, operationName);
        EnsureSuccessStatusCode(response);
        return JsonHelper.Deserialize<T>(response.Content ?? string.Empty);
    }

    protected static void EnsureSuccessStatusCode(RestResponse response)
    {
        if (!response.IsSuccessful)
        {
            throw new HttpRequestException(
                $"Request failed with status code {(int)response.StatusCode}: {response.Content}");
        }
    }
}
