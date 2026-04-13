using AutomationExercise.Tests.Infrastructure.Logging;
using AutomationExercise.Tests.PageObjects.Base;
using AutomationExercise.Tests.TestData.Builders;
using Microsoft.Playwright;

namespace AutomationExercise.Tests.PageObjects;

public sealed class RegisterPage : BasePage
{
    private const string TitleMrRadio       = "#id_gender1";
    private const string NameInput          = "#name";
    private const string PasswordInput      = "#password";
    private const string DaySelect          = "#days";
    private const string MonthSelect        = "#months";
    private const string YearSelect         = "#years";
    private const string FirstNameInput     = "#first_name";
    private const string LastNameInput      = "#last_name";
    private const string CompanyInput       = "#company";
    private const string AddressInput       = "#address1";
    private const string CountrySelect      = "#country";
    private const string StateInput         = "#state";
    private const string CityInput          = "#city";
    private const string ZipcodeInput       = "#zipcode";
    private const string MobileInput        = "#mobile_number";
    private const string CreateAccountBtn   = "button[data-qa='create-account']";
    private const string AccountCreatedMsg  = "h2[data-qa='account-created']";

    public RegisterPage(IPage page, ITestLogger logger) : base(page, logger)
    {
    }

    public override async Task<bool> IsLoadedAsync()
    {
        return await L(PasswordInput).IsVisibleAsync();
    }

    public async Task FillRegistrationFormAsync(UserModel user)
    {
        await ClickAsync(TitleMrRadio);
        await FillAsync(NameInput, user.Name);
        await FillAsync(PasswordInput, user.Password);
        await SelectAsync(DaySelect, "1");
        await SelectAsync(MonthSelect, "1");
        await SelectAsync(YearSelect, "1990");
        await FillAsync(FirstNameInput, user.FirstName);
        await FillAsync(LastNameInput, user.LastName);
        await FillAsync(CompanyInput, user.Company);
        await FillAsync(AddressInput, user.Address);
        await SelectAsync(CountrySelect, user.Country);
        await FillAsync(StateInput, user.State);
        await FillAsync(CityInput, user.City);
        await FillAsync(ZipcodeInput, user.Zipcode);
        await FillAsync(MobileInput, user.Phone);
    }

    private const string ContinueBtn = "a[data-qa='continue-button']";

    public async Task SubmitAsync()
    {
        await ClickAsync(CreateAccountBtn);
        await WaitForUrlAsync("/account_created");
    }

    public async Task ContinueAsync()
    {
        await ClickAsync(ContinueBtn);
        await Page.WaitForLoadStateAsync(Microsoft.Playwright.LoadState.NetworkIdle);
    }

    public async Task<bool> IsAccountCreatedAsync()
    {
        return await L(AccountCreatedMsg).IsVisibleAsync();
    }
}
