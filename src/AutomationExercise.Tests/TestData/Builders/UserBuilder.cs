using Bogus;
using Bogus.DataSets;

namespace AutomationExercise.Tests.TestData.Builders;

public sealed class UserBuilder
{
    private readonly Faker<UserModel> _faker;

    public UserBuilder(int? seed = null)
    {
        _faker = new Faker<UserModel>()
            .UseSeed(seed ?? Environment.TickCount)
            .RuleFor(u => u.Gender, f => f.PickRandom<Name.Gender>())
            .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName(u.Gender))
            .RuleFor(u => u.LastName, (f, u) => f.Name.LastName(u.Gender))
            .RuleFor(u => u.Name, (f, u) => $"{u.FirstName} {u.LastName}")
            .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
            .RuleFor(u => u.Password, f => f.Internet.Password(12, memorable: true))
            .RuleFor(u => u.Company, f => f.Company.CompanyName())
            .RuleFor(u => u.Address, f => f.Address.StreetAddress())
            .RuleFor(u => u.City, f => f.Address.City())
            .RuleFor(u => u.State, f => f.Address.State())
            .RuleFor(u => u.Zipcode, f => f.Address.ZipCode())
            .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber("##########"))
            .RuleFor(u => u.Country, _ => "United States");
    }

    public UserModel Build() => _faker.Generate();

    public IReadOnlyList<UserModel> BuildMany(int count) => _faker.Generate(count);

    public static UserModel AValidUser() => new UserBuilder().Build();

    public static UserModel AMinimalUser() => new UserBuilder(seed: 42).Build();
}
