using Bogus.DataSets;

namespace AutomationExercise.Tests.TestData.Builders;

public sealed class UserModel
{
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Company { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public string Zipcode { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string Country { get; init; } = "United States";
    public Name.Gender Gender { get; init; }
}
