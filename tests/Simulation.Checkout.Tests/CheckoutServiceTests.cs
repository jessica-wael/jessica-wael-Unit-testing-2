using FluentAssertions;
using Moq;
using Simulation.Checkout.Application;
using Simulation.Checkout.Application.Abstractions;
using Simulation.Checkout.Domain;
using Simulation.Checkout.Domain.Exceptions;
using Xunit;

namespace Simulation.Checkout.Tests;

public class CheckoutServiceTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
    private readonly Mock<IInventoryService> _inventoryServiceMock = new();
    private readonly Mock<INotificationService> _notficationServiceMock = new();
    private readonly CheckoutService _checkoutService;

    public CheckoutServiceTests()
    {
        _checkoutService = new CheckoutService(
            _orderRepositoryMock.Object,
            _inventoryServiceMock.Object,
            _notficationServiceMock.Object);

        _inventoryServiceMock
           .Setup(x => x.IsAvailableAsync(
            It.IsAny<Guid>(),
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()))
          .ReturnsAsync(true);
    }

    [Fact]
    public async Task CheckoutAsync_WhenOrderIsEmpty_ShouldThrowArgumentException()
    {
        // Arrange
        var order = new Order
        {
            CustomerName = "John Doe",
            CustomerEmail = "john.doe@example.com",
            Items = new List<OrderItem>()
        };

        // Act 
        var act = async () => await _checkoutService.CheckoutAsync(order);

        // Assert
        await act.Should().ThrowAsync<EmptyOrderException>();

    }


    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task CheckoutAsync_WhenQuantityIsZeroOrNegative_ShouldThrowArgumentException(int quantity)
    {
        //Arange 
        var order = new Order
        {
            CustomerName = "John Doe",
            CustomerEmail = "john.doe@example.com",
            Items = new List<OrderItem>
            {
                new OrderItem
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Laptop",
                    UnitPrice = 1000m,
                    Quantity = quantity
                }
            }
        };

        // Act
        var act = async () => await _checkoutService.CheckoutAsync(order);

        //Assert
        await act.Should().ThrowAsync<InvalidQuantityException>()
            .WithMessage("*Laptop*");
    }

    [Fact]
    public async Task CheckoutAsync_WhenPriceIsNegative_ShouldThrowInvalidPriceException()
    {
        // Arrange
        var order = new Order
        {
            CustomerName = "John Doe",
            CustomerEmail = "john.doe@example.com",
            Items = new List<OrderItem>
            {
                new OrderItem
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Laptop",
                    UnitPrice = -0.01m,
                    Quantity = 1
                }
            }
        };

        // Act
        var act = async () => await _checkoutService.CheckoutAsync(order);

        // Assert
        await act.Should().ThrowAsync<InvalidPriceException>();
    }

    [Fact]
    public async Task CheckoutAsync_WhenPriceIsZero_ShouldCalculateTotalPriceCorrectly()
    {
        // Arrange
        var order = new Order
        {
            CustomerName = "John Doe",
            CustomerEmail = "john.doe@example.com",
            Items = new List<OrderItem>
            {
                new OrderItem
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Laptop",
                    UnitPrice = 0m,
                    Quantity = 1
                }
            }
        };

        // Act
        var result = await _checkoutService.CheckoutAsync(order);

        // Assert

        result.Should().NotBeNull();
        result.Total.Should().Be(0m);
    }

    [Theory]
    [InlineData(999.99, 0)]
    [InlineData(1000, 100)]
    [InlineData(1500, 100)]
    public async Task CheckoutAsync_WhenDiscountIsApplied_ShouldCalculateTotalPriceCorrectly
        (decimal unitPrice, decimal expectedTotal)
    {
        // Arrange
        var order = new Order
        {
            CustomerName = "John Doe",
            CustomerEmail = "john.doe@example.com",
            Items = new List<OrderItem>
            {
                new OrderItem
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Laptop",
                    UnitPrice = unitPrice,
                    Quantity = 1
                }
            }
        };

        // Act
        var result = await _checkoutService.CheckoutAsync(order);

        // Assert
        result.Discount.Should().Be(expectedTotal);

    }

    [Fact]
    public async Task CheckoutAsync_WhenOrderMatchesExampleA_ShouldReturnCorrectTotals()
    {
        // Arrange

        var order = new Order
        {
            CustomerName = "John Doe",
            CustomerEmail = "john.doe@example.com",
            Items = new List<OrderItem>
            {
                new OrderItem
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Laptop",
                    UnitPrice = 400m,
                    Quantity = 1
                },
                new OrderItem
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Mouse",
                    UnitPrice = 150m,
                    Quantity = 2
                }
            }
        };

        // Act
        var result = await _checkoutService.CheckoutAsync(order);

        // Assert
        result.Subtotal.Should().Be(700m);
        result.Discount.Should().Be(0m);
        result.Total.Should().Be(700m);

    }


    [Fact]
    public async Task CheckoutAsync_WhenOrderMatchesExampleC_ShouldCapDiscountAtMax()
    {
        // Arrange

        var order = new Order
        {
            CustomerName = "John Doe",
            CustomerEmail = "john.doe@example.com",
            Items = new List<OrderItem>
            {
                new OrderItem
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Monitor",
                    UnitPrice = 500m,
                    Quantity = 3
                }
            }
        };

        // Act
        var result = await _checkoutService.CheckoutAsync(order);

        // Assert
        result.Subtotal.Should().Be(1500m);
        result.Discount.Should().Be(100m);
        result.Total.Should().Be(1400m);

    }

}

