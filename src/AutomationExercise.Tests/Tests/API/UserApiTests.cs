using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using AutomationExercise.Tests.ApiClients;
using AutomationExercise.Tests.Infrastructure.Base;
using AutomationExercise.Tests.TestData.Builders;
using FluentAssertions;
using NUnit.Framework;

namespace AutomationExercise.Tests.Tests.API;

[TestFixture, Category("API")]
[AllureNUnit]
[AllureSuite("User API")]
[AllureFeature("User CRUD Operations")]
public sealed class UserApiTests : BaseApiTest
{
    private UserApiClient _client = null!;
    private UserModel? _createdUser;

    protected override Task OnSetUpAsync()
    {
        _client = Resolve<UserApiClient>();
        return Task.CompletedTask;
    }

    protected override async Task OnTearDownAsync(bool testFailed)
    {
        if (_createdUser is not null)
        {
            await _client.DeleteUserAsync(_createdUser.Email, _createdUser.Password);
            _createdUser = null;
        }
    }

    [Test]
    [AllureSeverity(SeverityLevel.blocker)]
    [AllureDescription("Verifies that creating a user with valid data returns HTTP 201 and a success message.")]
    public async Task CreateUser_WithValidData_ShouldReturn201()
    {
        _createdUser = UserBuilder.AValidUser();

        var response = await _client.CreateUserAsync(_createdUser);

        response.Should().NotBeNull();
        response!.ResponseCode.Should().Be(201);
        response.Message.Should().Be("User created!");
    }

    [Test]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureDescription("Verifies that login with valid credentials returns HTTP 200 and confirms user exists.")]
    public async Task VerifyLogin_WithValidCredentials_ShouldReturn200()
    {
        _createdUser = UserBuilder.AValidUser();
        await _client.CreateUserAsync(_createdUser);

        var response = await _client.VerifyLoginAsync(_createdUser.Email, _createdUser.Password);

        response.Should().NotBeNull();
        response!.ResponseCode.Should().Be(200);
        response.Message.Should().Be("User exists!");
    }

    [Test, Category("Negative")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureDescription("Verifies that login with invalid credentials returns HTTP 404.")]
    public async Task VerifyLogin_WithInvalidCredentials_ShouldReturn404()
    {
        var response = await _client.VerifyLoginAsync("nobody@nowhere.invalid", "WrongPassword999!");

        response.Should().NotBeNull();
        response!.ResponseCode.Should().Be(404);
    }

    [Test]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureDescription("Verifies that deleting an existing user returns HTTP 200 and a deletion confirmation.")]
    public async Task DeleteUser_ExistingUser_ShouldReturn200()
    {
        var user = UserBuilder.AValidUser();
        await _client.CreateUserAsync(user);

        var response = await _client.DeleteUserAsync(user.Email, user.Password);

        response.Should().NotBeNull();
        response!.ResponseCode.Should().Be(200);
        response.Message.Should().Be("Account deleted!");
    }
}
