using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using AutomationExercise.Tests.ApiClients;
using AutomationExercise.Tests.Infrastructure.Base;
using AutomationExercise.Tests.PageObjects;
using AutomationExercise.Tests.TestData.Builders;
using FluentAssertions;
using NUnit.Framework;

namespace AutomationExercise.Tests.Tests.DataDriven;

[TestFixture, Category("DataDriven")]
[AllureNUnit]
[AllureSuite("Data-Driven Tests")]
public sealed class LoginDataDrivenTests : BaseUiTest
{
    private LoginPage _loginPage = null!;

    /// <summary>Scenarios where login is expected to fail.</summary>
    public static object[][] FailedLoginScenarios =
    [
        ["wronguser@automation.com", "Test@12345", "Unknown email"],
        ["",                         "Test@12345", "Empty email"],
        ["wronguser@automation.com", "",            "Empty password"],
        ["wronguser@automation.com", "wrongpass",   "Wrong password"],
    ];

    protected override async Task OnSetUpAsync()
    {
        await base.OnSetUpAsync();
        _loginPage = new LoginPage(Page, Logger);
        await NavigateToAsync("/login");
    }

    [Test]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureDescription("Verifies that a registered user can log in successfully using valid credentials.")]
    public async Task Login_ValidCredentials_ShouldSucceed()
    {
        var user = UserBuilder.AValidUser();
        var apiClient = Resolve<UserApiClient>();

        await apiClient.CreateUserAsync(user);
        try
        {
            Logger.LogStep($"Logging in as {user.Email}");
            await _loginPage.LoginAsync(user.Email, user.Password);

            var isLoggedIn = await _loginPage.Navbar.IsUserLoggedInAsync();
            isLoggedIn.Should().BeTrue(because: "user was just registered via API");
        }
        finally
        {
            await apiClient.DeleteUserAsync(user.Email, user.Password);
        }
    }

    [Test, TestCaseSource(nameof(FailedLoginScenarios))]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureDescription("Verifies that login fails and surfaces an error for invalid credential combinations.")]
    public async Task Login_InvalidCredentials_ShouldFail(
        string email, string password, string description)
    {
        Logger.LogStep($"Scenario: {description}");

        await _loginPage.LoginAsync(email, password);

        var isErrorVisible = await _loginPage.IsLoginErrorVisibleAsync();
        var url = Page.Url;
        (isErrorVisible || url.Contains("/login")).Should().BeTrue(because: description);
    }
}
