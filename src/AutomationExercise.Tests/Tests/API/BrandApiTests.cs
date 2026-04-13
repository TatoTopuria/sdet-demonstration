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
[AllureSuite("Brand API")]
[AllureFeature("Brand Endpoints")]
public sealed class BrandApiTests : BaseApiTest
{
    private BrandApiClient _client = null!;

    protected override Task OnSetUpAsync()
    {
        _client = Resolve<BrandApiClient>();
        return Task.CompletedTask;
    }

    [Test]
    [AllureSeverity(SeverityLevel.blocker)]
    [AllureDescription("Verifies that GET brandsList returns HTTP 200 and a non-empty brands list.")]
    public async Task GetAllBrands_ShouldReturn200WithBrands()
    {
        var response = await _client.GetAllBrandsAsync();

        response.Should().NotBeNull();
        response!.ResponseCode.Should().Be(200);
        response.Brands.Should().NotBeEmpty();
    }

    [Test]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureDescription("Verifies that every brand in the list has a valid id and a non-empty name.")]
    public async Task GetAllBrands_EachBrand_ShouldHaveNameAndId()
    {
        var response = await _client.GetAllBrandsAsync();

        response.Should().NotBeNull();
        foreach (var brand in response!.Brands)
        {
            brand.Id.Should().BeGreaterThan(0);
            brand.Name.Should().NotBeNullOrWhiteSpace();
        }
    }

    [Test, Category("Negative")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureDescription("Verifies that a PUT request to brandsList is rejected with HTTP 405.")]
    public async Task PutToGetAllBrands_ShouldReturn405()
    {
        var response = await _client.PutToGetAllBrandsAsync();

        response.Should().NotBeNull();
        response!.ResponseCode.Should().Be(405);
    }
}
