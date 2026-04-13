using AutomationExercise.Tests.Infrastructure.DI;
using AutomationExercise.Tests.Infrastructure.Logging;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace AutomationExercise.Tests.Infrastructure.Base;

[TestFixture]
public abstract class BaseTest
{
    protected ServiceProvider? _serviceProvider;

    protected IServiceScope TestScope { get; private set; } = null!;
    protected ITestLogger Logger { get; private set; } = null!;

    protected T Resolve<T>() where T : notnull
        => TestScope.ServiceProvider.GetRequiredService<T>();

    [OneTimeSetUp]
    public virtual void OneTimeSetUp()
    {
        var configuration = ServiceCollectionExtensions.BuildConfiguration();
        var services = new ServiceCollection();
        services.AddTestInfrastructure(configuration);
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();
    }

    [SetUp]
    public virtual async Task SetUp()
    {
        TestScope = _serviceProvider!.CreateScope();
        Logger = Resolve<ITestLogger>();
        Logger.LogInfo($"Test started: {TestContext.CurrentContext.Test.FullName}");
        await OnSetUpAsync();
    }

    [TearDown]
    public virtual async Task TearDown()
    {
        var status = TestContext.CurrentContext.Result.Outcome.Status;
        Logger.LogInfo($"Test finished: {status}");
        await OnTearDownAsync(testFailed: status == TestStatus.Failed);
        TestScope.Dispose();
    }

    [OneTimeTearDown]
    public virtual void OneTimeTearDown()
    {
        _serviceProvider?.Dispose();
    }

    protected virtual void ConfigureServices(IServiceCollection services) { }

    protected virtual Task OnSetUpAsync() => Task.CompletedTask;

    protected virtual Task OnTearDownAsync(bool testFailed) => Task.CompletedTask;
}
