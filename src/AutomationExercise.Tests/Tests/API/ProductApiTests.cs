using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using AutomationExercise.Tests.ApiClients;
using AutomationExercise.Tests.Infrastructure.Base;
using FluentAssertions;
using NUnit.Framework;

namespace AutomationExercise.Tests.Tests.API;

[TestFixture, Category("API")]
[AllureNUnit]
[AllureSuite("Product API")]
[AllureFeature("Product Endpoints")]
public sealed class ProductApiTests : BaseApiTest
{
    private ProductApiClient _client = null!;

    protected override Task OnSetUpAsync()
    {
        _client = Resolve<ProductApiClient>();
        return Task.CompletedTask;
    }

    [Test]
    [AllureSeverity(SeverityLevel.blocker)]
    [AllureDescription("Verifies that GET productsList returns HTTP 200 and a non-empty products list.")]
    public async Task GetAllProducts_ShouldReturn200WithProducts()
    {
        var response = await _client.GetAllProductsAsync();

        response.Should().NotBeNull();
        response!.ResponseCode.Should().Be(200);
        response.Products.Should().NotBeEmpty();
    }

    [Test]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureDescription("Verifies that every product in the list has a valid id, name, price, and brand.")]
    public async Task GetAllProducts_EachProduct_ShouldHaveRequiredFields()
    {
        var response = await _client.GetAllProductsAsync();

        response.Should().NotBeNull();
        foreach (var product in response!.Products)
        {
            product.Id.Should().BeGreaterThan(0);
            product.Name.Should().NotBeNullOrWhiteSpace();
            product.Price.Should().NotBeNullOrWhiteSpace();
            product.Brand.Should().NotBeNullOrWhiteSpace();
        }
    }

    [Test, Category("Negative")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureDescription("Verifies that a POST request to productsList is rejected with HTTP 405.")]
    public async Task PostToGetAllProducts_ShouldReturn405()
    {
        var response = await _client.PostToGetAllProductsAsync();

        response.Should().NotBeNull();
        response!.ResponseCode.Should().Be(405);
    }

    [Test]
    [TestCase("top")]
    [TestCase("jeans")]
    [TestCase("shirt")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureDescription("Verifies that searching products by keyword returns a non-empty matching list.")]
    public async Task SearchProducts_WithKeyword_ShouldReturnMatchingProducts(string keyword)
    {
        var response = await _client.SearchProductsAsync(keyword);

        response.Should().NotBeNull();
        response!.ResponseCode.Should().Be(200);
        response.Products.Should().NotBeEmpty();
    }
}
