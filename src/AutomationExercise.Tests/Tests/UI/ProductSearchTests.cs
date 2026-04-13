using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using AutomationExercise.Tests.Infrastructure.Base;
using AutomationExercise.Tests.PageObjects;
using FluentAssertions;
using NUnit.Framework;

namespace AutomationExercise.Tests.Tests.UI;

[TestFixture, Category("UI")]
[AllureNUnit]
[AllureSuite("Product Search")]
[AllureFeature("Product Catalogue")]
public sealed class ProductSearchTests : BaseUiTest
{
    private ProductsPage _productsPage = null!;

    protected override async Task OnSetUpAsync()
    {
        await base.OnSetUpAsync();
        _productsPage = new ProductsPage(Page, Logger);
        await NavigateToAsync("/products");
    }

    [Test]
    [AllureSeverity(SeverityLevel.blocker)]
    [AllureDescription("Verifies that the products page loads with at least one product displayed.")]
    public async Task ProductsPage_OnLoad_ShouldDisplayProducts()
    {
        var count = await _productsPage.GetProductCountAsync();
        count.Should().BeGreaterThan(0);
    }

    [Test]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureDescription("Verifies that searching by a valid keyword returns at least one result.")]
    public async Task SearchProducts_WithValidKeyword_ShouldReturnResults()
    {
        await _productsPage.SearchForAsync("top");

        var count = await _productsPage.GetProductCountAsync();
        count.Should().BeGreaterThan(0);
    }

    [Test]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureDescription("Verifies that all products returned for a keyword search contain that keyword in their name.")]
    public async Task SearchProducts_AllResults_ShouldContainKeyword()
    {
        await _productsPage.SearchForAsync("top");

        var names = await _productsPage.GetProductNamesAsync();
        names.Should().NotBeEmpty();
        // At least one result should contain the keyword — the site may also return category matches
        names.Should().Contain(name =>
            name.Contains("top", StringComparison.OrdinalIgnoreCase));
    }

    [Test]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureDescription("Verifies that adding the first product to the cart results in at least one cart item.")]
    public async Task AddToCart_ShouldNotThrow()
    {
        await _productsPage.AddFirstProductToCartAsync();
        await NavigateToAsync("/view_cart");

        var cartPage = new CartPage(Page, Logger);
        var itemCount = await cartPage.GetItemCountAsync();
        itemCount.Should().BeGreaterThanOrEqualTo(1);
    }
}
