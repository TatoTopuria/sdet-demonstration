using AutomationExercise.Tests.Configuration;
using Microsoft.Extensions.Options;

namespace AutomationExercise.Tests.Infrastructure.Base;

public abstract class BaseApiTest : BaseTest
{
    protected AppSettings AppSettings { get; private set; } = null!;

    protected override Task OnSetUpAsync()
    {
        AppSettings = Resolve<IOptions<AppSettings>>().Value;
        return Task.CompletedTask;
    }
}
