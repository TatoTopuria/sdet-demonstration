namespace AutomationExercise.Tests.ApiClients.Base;

public interface IApiClient<TResponse>
{
    Task<TResponse> GetAllAsync();
}

public interface ICrudApiClient<TRequest, TResponse> : IApiClient<TResponse>
{
    Task<TResponse> GetByIdAsync(int id);
    Task<TResponse> CreateAsync(TRequest request);
    Task<TResponse> DeleteAsync(int id);
}
