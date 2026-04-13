using AutomationExercise.Tests.ApiClients.Base;
using AutomationExercise.Tests.ApiClients.Models.Responses;
using AutomationExercise.Tests.Configuration;
using AutomationExercise.Tests.Infrastructure.Retry;
using AutomationExercise.Tests.TestData.Builders;
using Microsoft.Extensions.Options;
using RestSharp;

namespace AutomationExercise.Tests.ApiClients;

public sealed class UserApiClient : BaseApiClient
{
    public UserApiClient(
        IOptions<AppSettings> appSettings,
        IOptions<ApiSettings> apiSettings,
        RetryPolicy retryPolicy)
        : base(appSettings, apiSettings, retryPolicy) { }

    public async Task<ApiResponse?> CreateUserAsync(UserModel user)
    {
        var request = new RestRequest("createAccount", Method.Post);
        request.AddParameter("name",         user.Name);
        request.AddParameter("email",        user.Email);
        request.AddParameter("password",     user.Password);
        request.AddParameter("title",        "Mr");
        request.AddParameter("birth_date",   "1");
        request.AddParameter("birth_month",  "January");
        request.AddParameter("birth_year",   "1990");
        request.AddParameter("firstname",    user.FirstName);
        request.AddParameter("lastname",     user.LastName);
        request.AddParameter("company",      user.Company);
        request.AddParameter("address1",     user.Address);
        request.AddParameter("country",      user.Country);
        request.AddParameter("zipcode",      user.Zipcode);
        request.AddParameter("state",        user.State);
        request.AddParameter("city",         user.City);
        request.AddParameter("mobile_number",user.Phone);
        return await ExecuteAndDeserializeAsync<ApiResponse>(request, nameof(CreateUserAsync));
    }

    public async Task<ApiResponse?> GetUserByEmailAsync(string email)
    {
        var request = new RestRequest("getUserDetailByEmail", Method.Get);
        request.AddQueryParameter("email", email);
        return await ExecuteAndDeserializeAsync<ApiResponse>(request, nameof(GetUserByEmailAsync));
    }

    public async Task<ApiResponse?> DeleteUserAsync(string email, string password)
    {
        var request = new RestRequest("deleteAccount", Method.Delete);
        // DELETE + AddParameter sends query-string; the API requires form body
        request.AddStringBody(
            $"email={Uri.EscapeDataString(email)}&password={Uri.EscapeDataString(password)}",
            "application/x-www-form-urlencoded");
        return await ExecuteAndDeserializeAsync<ApiResponse>(request, nameof(DeleteUserAsync));
    }

    public async Task<ApiResponse?> UpdateUserAsync(UserModel user, string password)
    {
        var request = new RestRequest("updateAccount", Method.Put);
        request.AddParameter("name",      user.Name);
        request.AddParameter("email",     user.Email);
        request.AddParameter("password",  password);
        request.AddParameter("firstname", user.FirstName);
        request.AddParameter("lastname",  user.LastName);
        return await ExecuteAndDeserializeAsync<ApiResponse>(request, nameof(UpdateUserAsync));
    }

    public async Task<ApiResponse?> VerifyLoginAsync(string email, string password)
    {
        var request = new RestRequest("verifyLogin", Method.Post);
        request.AddParameter("email",    email);
        request.AddParameter("password", password);
        return await ExecuteAndDeserializeAsync<ApiResponse>(request, nameof(VerifyLoginAsync));
    }
}
