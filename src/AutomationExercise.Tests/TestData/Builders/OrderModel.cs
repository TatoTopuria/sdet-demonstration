namespace AutomationExercise.Tests.TestData.Builders;

public sealed class OrderModel
{
    public string CardNumber { get; init; } = string.Empty;
    public string CardHolder { get; init; } = string.Empty;
    public string ExpiryMonth { get; init; } = string.Empty;
    public string ExpiryYear { get; init; } = string.Empty;
    public string Cvv { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
}
