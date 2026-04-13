using AutomationExercise.Tests.Infrastructure.Logging;
using AutomationExercise.Tests.PageObjects.Base;
using AutomationExercise.Tests.PageObjects.Components;
using Microsoft.Playwright;

namespace AutomationExercise.Tests.PageObjects;

public sealed class ProductsPage : BasePage
{
    private const string SearchInput         = "input#search_product";
    private const string SearchButton        = "button#submit_search";
    private const string ProductCards        = ".productinfo";
    private const string ProductName         = ".productinfo p";
    private const string AddToCartBtn        = "a[data-product-id]";
    private const string ContinueShoppingBtn = "button.close-modal";

    public NavbarComponent Navbar { get; }

    public ProductsPage(IPage page, ITestLogger logger) : base(page, logger)
    {
        Navbar = new NavbarComponent(page, logger);
    }

    public override async Task<bool> IsLoadedAsync()
    {
        return await L(SearchInput).IsVisibleAsync();
    }

    public async Task SearchForAsync(string keyword)
    {
        await FillAsync(SearchInput, keyword);
        await ClickAsync(SearchButton);
    }

    public async Task<int> GetProductCountAsync()
    {
        return await L(ProductCards).CountAsync();
    }

    public async Task<IReadOnlyList<string>> GetProductNamesAsync()
    {
        return await L(ProductName).AllInnerTextsAsync();
    }

    public async Task AddFirstProductToCartAsync()
    {
        await AddProductByIndexToCartAsync(0);
    }

    public async Task AddProductByIndexToCartAsync(int index)
    {
        // Hover to make the overlay visible before clicking the add-to-cart button
        await L(".product-image-wrapper").Nth(index).HoverAsync();
        await L(AddToCartBtn).Nth(index).ClickAsync();
        await ClickAsync(ContinueShoppingBtn);
    }
}
