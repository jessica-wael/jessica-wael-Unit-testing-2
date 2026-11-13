using Bogus;
using FluentAssertions;
using Moq;
using AutoFixture;
using Simulation.Checkout.Application;
using Simulation.Checkout.Application.Abstractions;
using Simulation.Checkout.Domain;
using Simulation.Checkout.Domain.Exceptions;
using Simulation.Checkout.Tests.TestData;
using Xunit;

namespace Simulation.Checkout.Tests;

public class ReceiptSnapshotTests
{
	[Fact]
	public async Task CheckoutAsync_ForExampleC_ShouldMatchReceiptSnapshot()
	{
        // Arrange
		var inventory = new Mock<IInventoryService>();

		inventory
			.Setup(x => x.IsAvailableAsync(
				It.IsAny<Guid>(),
				It.IsAny<int>(),
				It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

		var service = new CheckoutService(
			new Mock<IOrderRepository>().Object,
            inventory.Object,
			new Mock<INotificationService>().Object);

        var order = new Order
        {
            CustomerName= "John Doe",
            CustomerEmail = "john.doe@example.com",

            Items = new List<OrderItem>
            {
                new OrderItem
                {
                    ProductId = Guid.Parse("12345678-1234-1234-1234-123456789012"),
                    ProductName = "Monitor",
                    UnitPrice = 500m,
                    Quantity = 3
                }
            }
        };

        // Act 
        var receipt = await service.CheckoutAsync(order);

        // Assert

        await Verify(receipt);
            
    }
}
