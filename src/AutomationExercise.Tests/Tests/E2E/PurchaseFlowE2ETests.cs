using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using AutomationExercise.Tests.ApiClients;
using AutomationExercise.Tests.Infrastructure.Base;
using AutomationExercise.Tests.PageObjects;
using AutomationExercise.Tests.TestData.Builders;
using FluentAssertions;
using NUnit.Framework;

namespace AutomationExercise.Tests.Tests.E2E;

[TestFixture, Category("E2E")]
[AllureNUnit]
[AllureSuite("Purchase Flow")]
[AllureFeature("End-to-End Shopping")]
public sealed class PurchaseFlowE2ETests : BaseUiTest
{
    private LoginPage _loginPage = null!;
    private ProductsPage _productsPage = null!;
    private CartPage _cartPage = null!;
    private CheckoutPage _checkoutPage = null!;
    private UserApiClient _userApiClient = null!;
    private UserModel? _testUser;

    protected override async Task OnSetUpAsync()
    {
        await base.OnSetUpAsync();

        _loginPage    = new LoginPage(Page, Logger);
        _productsPage = new ProductsPage(Page, Logger);
        _cartPage     = new CartPage(Page, Logger);
        _checkoutPage = new CheckoutPage(Page, Logger);

        _userApiClient = Resolve<UserApiClient>();

        _testUser = UserBuilder.AValidUser();
        var createResponse = await _userApiClient.CreateUserAsync(_testUser);
        createResponse!.ResponseCode.Should().Be(201);
    }

    protected override async Task OnTearDownAsync(bool testFailed)
    {
        if (_testUser is not null)
        {
            await _userApiClient.DeleteUserAsync(_testUser.Email, _testUser.Password);
        }

        await base.OnTearDownAsync(testFailed);
    }

    [Test]
    [AllureSeverity(SeverityLevel.blocker)]
    [AllureDescription("Verifies the complete purchase flow from login through order confirmation for a registered user.")]
    public async Task FullPurchaseFlow_RegisteredUser_ShouldCompleteSuccessfully()
    {
        Logger.LogStep("1. Navigate to login and authenticate as test user");
        await NavigateToAsync("/login");
        await _loginPage.LoginAsync(_testUser!.Email, _testUser.Password);
        var isLoggedIn = await _loginPage.Navbar.IsUserLoggedInAsync();
        isLoggedIn.Should().BeTrue();

        Logger.LogStep("2. Navigate to products and search for 'top'");
        await NavigateToAsync("/products");
        await _productsPage.SearchForAsync("top");
        var productCount = await _productsPage.GetProductCountAsync();
        productCount.Should().BeGreaterThan(0);

        Logger.LogStep("3. Add the first product to cart");
        await _productsPage.AddFirstProductToCartAsync();

        Logger.LogStep("4. Navigate to cart and verify item count is 1");
        await NavigateToAsync("/view_cart");
        var itemCount = await _cartPage.GetItemCountAsync();
        itemCount.Should().Be(1);

        Logger.LogStep("5. Proceed to checkout and enter order comment");
        await _cartPage.ProceedToCheckoutAsync();
        await _checkoutPage.EnterCommentAndPlaceOrderAsync("E2E test order");

        Logger.LogStep("6. Fill in payment details");
        await _checkoutPage.FillPaymentDetailsAsync(OrderBuilder.AValidOrder());

        Logger.LogStep("7. Assert order was placed successfully");
        var isOrderSuccessful = await _checkoutPage.IsOrderSuccessfulAsync();
        isOrderSuccessful.Should().BeTrue();

        Logger.LogStep("8. Capture order confirmation screenshot");
        await CaptureScreenshotAsync("order-confirmation");
    }
}
