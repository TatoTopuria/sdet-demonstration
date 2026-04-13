using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using AutomationExercise.Tests.ApiClients;
using AutomationExercise.Tests.Infrastructure.Base;
using AutomationExercise.Tests.PageObjects;
using AutomationExercise.Tests.TestData.Builders;
using FluentAssertions;
using NUnit.Framework;

namespace AutomationExercise.Tests.Tests.UI;

[TestFixture, Category("UI")]
[AllureNUnit]
[AllureSuite("Authentication")]
[AllureFeature("User Authentication")]
public sealed class AuthenticationTests : BaseUiTest
{
    private LoginPage _loginPage = null!;
    private RegisterPage _registerPage = null!;

    protected override async Task OnSetUpAsync()
    {
        await base.OnSetUpAsync();
        _loginPage = new LoginPage(Page, Logger);
        _registerPage = new RegisterPage(Page, Logger);
        await NavigateToAsync("/login");
    }

    [Test]
    [AllureSeverity(SeverityLevel.blocker)]
    [AllureDescription("Verifies that a user registered via API can log in through the UI.")]
    public async Task Login_WithValidCredentials_ShouldSucceed()
    {
        var user = UserBuilder.AValidUser();
        var apiClient = Resolve<UserApiClient>();
        await apiClient.CreateUserAsync(user);
        try
        {
            await _loginPage.LoginAsync(user.Email, user.Password);

            var isLoggedIn = await _loginPage.Navbar.IsUserLoggedInAsync();
            isLoggedIn.Should().BeTrue();
        }
        finally
        {
            await apiClient.DeleteUserAsync(user.Email, user.Password);
        }
    }

    [Test]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureDescription("Verifies that an incorrect password displays the login error message.")]
    public async Task Login_WithInvalidPassword_ShouldShowError()
    {
        await _loginPage.LoginAsync("nobody@automation-test.io", "WrongPassword!");

        var isErrorVisible = await _loginPage.IsLoginErrorVisibleAsync();
        isErrorVisible.Should().BeTrue();
    }

    [Test]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureDescription("Verifies that a newly registered user is logged in and can subsequently log out.")]
    public async Task Registration_NewUser_ShouldLoginAfterRegistration()
    {
        var user = UserBuilder.AValidUser();
        var apiClient = Resolve<UserApiClient>();
        try
        {
            await _loginPage.SignUpAsync(user.Name, user.Email);
            await _registerPage.FillRegistrationFormAsync(user);
            await _registerPage.SubmitAsync();
            await _registerPage.ContinueAsync();

            var isLoggedIn = await _loginPage.Navbar.IsUserLoggedInAsync();
            isLoggedIn.Should().BeTrue();

            await _loginPage.Navbar.LogoutAsync();

            var isLoggedInAfterLogout = await _loginPage.Navbar.IsUserLoggedInAsync();
            isLoggedInAfterLogout.Should().BeFalse();
        }
        finally
        {
            await apiClient.DeleteUserAsync(user.Email, user.Password);
        }
    }
}
