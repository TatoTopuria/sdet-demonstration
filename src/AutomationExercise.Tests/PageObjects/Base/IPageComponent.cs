namespace AutomationExercise.Tests.PageObjects.Base;

public interface IPageComponent
{
    Task<bool> IsLoadedAsync();
}
