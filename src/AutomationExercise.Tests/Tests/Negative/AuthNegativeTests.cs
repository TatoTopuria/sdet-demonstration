using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using AutomationExercise.Tests.ApiClients;
using AutomationExercise.Tests.Infrastructure.Base;
using AutomationExercise.Tests.TestData.Builders;
using FluentAssertions;
using NUnit.Framework;

namespace AutomationExercise.Tests.Tests.Negative;

[TestFixture, Category("Negative")]
[AllureNUnit]
[AllureSuite("Negative Tests")]
[AllureFeature("Authentication Boundaries")]
public sealed class AuthNegativeTests : BaseApiTest
{
    private UserApiClient _client = null!;

    protected override Task OnSetUpAsync()
    {
        _client = Resolve<UserApiClient>();
        return Task.CompletedTask;
    }

    [Test]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureDescription("Verifies that registering with a duplicate email returns HTTP 400 with an 'exists' message.")]
    public async Task CreateUser_DuplicateEmail_ShouldReturn400()
    {
        var user = UserBuilder.AValidUser();
        var first = await _client.CreateUserAsync(user);
        first!.ResponseCode.Should().Be(201);

        try
        {
            var second = await _client.CreateUserAsync(user);
            second.Should().NotBeNull();
            second!.ResponseCode.Should().Be(400);
            second.Message.Should().Contain("exists", Exactly.Once());
        }
        finally
        {
            await _client.DeleteUserAsync(user.Email, user.Password);
        }
    }

    [Test]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureDescription("Verifies that attempting to verify login with a missing password returns a non-success response code.")]
    public async Task VerifyLogin_MissingPassword_ShouldReturn400()
    {
        var response = await _client.VerifyLoginAsync("test@test.com", "");

        response.Should().NotBeNull();
        response!.ResponseCode.Should().Be(404);
    }

    [Test]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureDescription("Verifies that deleting a non-existent account returns HTTP 404.")]
    public async Task DeleteUser_NonExistentAccount_ShouldReturn404()
    {
        var response = await _client.DeleteUserAsync(
            $"ghost-{Guid.NewGuid():N}@test.invalid",
            "SomePassword123");

        response.Should().NotBeNull();
        response!.ResponseCode.Should().Be(404);
    }
}
