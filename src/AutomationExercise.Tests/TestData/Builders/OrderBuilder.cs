using Bogus;

namespace AutomationExercise.Tests.TestData.Builders;

public sealed class OrderBuilder
{
    private readonly Faker<OrderModel> _faker;

    public OrderBuilder()
    {
        _faker = new Faker<OrderModel>()
            .RuleFor(o => o.CardNumber, f => f.Finance.CreditCardNumber())
            .RuleFor(o => o.CardHolder, f => f.Name.FullName())
            .RuleFor(o => o.ExpiryMonth, f => f.Date.Future().Month.ToString("D2"))
            .RuleFor(o => o.ExpiryYear, f => f.Date.Future(5).Year.ToString())
            .RuleFor(o => o.Cvv, f => f.Finance.CreditCardCvv())
            .RuleFor(o => o.Message, f => f.Lorem.Sentence(5));
    }

    public OrderModel Build() => _faker.Generate();

    public static OrderModel AValidOrder() => new OrderBuilder().Build();
}
