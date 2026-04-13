using AutomationExercise.Tests.ApiClients.Base;
using AutomationExercise.Tests.ApiClients.Models.Responses;
using AutomationExercise.Tests.Configuration;
using AutomationExercise.Tests.Infrastructure.Retry;
using AutomationExercise.Tests.Utilities;
using Microsoft.Extensions.Options;
using RestSharp;

namespace AutomationExercise.Tests.ApiClients;

public sealed class BrandApiClient : BaseApiClient
{
    public BrandApiClient(
        IOptions<AppSettings> appSettings,
        IOptions<ApiSettings> apiSettings,
        RetryPolicy retryPolicy)
        : base(appSettings, apiSettings, retryPolicy) { }

    public async Task<BrandListResponse?> GetAllBrandsAsync()
    {
        var request = new RestRequest("brandsList", Method.Get);
        return await ExecuteAndDeserializeAsync<BrandListResponse>(request, nameof(GetAllBrandsAsync));
    }

    public async Task<ApiResponse?> PutToGetAllBrandsAsync()
    {
        var request = new RestRequest("brandsList", Method.Put);
        var response = await ExecuteAsync(request, nameof(PutToGetAllBrandsAsync));
        return JsonHelper.Deserialize<ApiResponse>(response.Content ?? string.Empty);
    }
}
