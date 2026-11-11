using AutoFixture;
using Bogus;
using Simulation.Checkout.Domain;
using System;

namespace Simulation.Checkout.Tests.TestData;


public class OrderTestData
{
    public static Fixture CreateFixture()
    {
        var fixture = new Fixture();

        fixture.Customize<OrderItem>(c => c
            .With(i => i.Quantity, () => Random.Shared.Next(1, 100))
            .With(i => i.UnitPrice, () => Random.Shared.Next(0, 100_000) / 100m));

        return fixture;
    }


    public static Faker<Order> OrderFaker() => new Faker<Order>()
        .UseSeed(1234)
        .CustomInstantiator(f =>
        {
            var person = f.Person;

            return new Order
            {
                CustomerName = person.FullName,
                CustomerEmail = person.Email,
                Items = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductId = f.Random.Guid(),
                        ProductName = f.Commerce.ProductName(),
                        UnitPrice = f.Random.Decimal(1m, 500m),
                        Quantity = f.Random.Int(1, 5)
                    }
                }
            };
        });
}
