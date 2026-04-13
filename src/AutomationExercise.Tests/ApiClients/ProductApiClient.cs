using AutomationExercise.Tests.ApiClients.Base;
using AutomationExercise.Tests.ApiClients.Models.Responses;
using AutomationExercise.Tests.Configuration;
using AutomationExercise.Tests.Infrastructure.Retry;
using AutomationExercise.Tests.Utilities;
using Microsoft.Extensions.Options;
using RestSharp;

namespace AutomationExercise.Tests.ApiClients;

public sealed class ProductApiClient : BaseApiClient
{
    public ProductApiClient(
        IOptions<AppSettings> appSettings,
        IOptions<ApiSettings> apiSettings,
        RetryPolicy retryPolicy)
        : base(appSettings, apiSettings, retryPolicy) { }

    public async Task<ProductListResponse?> GetAllProductsAsync()
    {
        var request = new RestRequest("productsList", Method.Get);
        return await ExecuteAndDeserializeAsync<ProductListResponse>(request, nameof(GetAllProductsAsync));
    }

    public async Task<ApiResponse?> PostToGetAllProductsAsync()
    {
        var request = new RestRequest("productsList", Method.Post);
        var response = await ExecuteAsync(request, nameof(PostToGetAllProductsAsync));
        return JsonHelper.Deserialize<ApiResponse>(response.Content ?? string.Empty);
    }

    public async Task<ProductListResponse?> SearchProductsAsync(string keyword)
    {
        var request = new RestRequest("searchProduct", Method.Post);
        request.AddParameter("search_product", keyword);
        return await ExecuteAndDeserializeAsync<ProductListResponse>(request, nameof(SearchProductsAsync));
    }
}
