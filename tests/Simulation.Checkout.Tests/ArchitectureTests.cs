using FluentAssertions;
using NetArchTest.Rules;
using Simulation.Checkout.Application;
using Simulation.Checkout.Domain;
using Simulation.Checkout.Infrastructure;
using Simulation.Checkout.Application.Abstractions;
using System.Reflection;
using Xunit;

namespace Simulation.Checkout.Tests;

public class ArchitectureTests
{
	private static readonly Assembly DomainAssembly = typeof(Order).Assembly;
	private static readonly Assembly ApplicationAssembly = typeof(CheckoutService).Assembly;
	private static readonly Assembly InfrastructureAssembly = typeof(InMemoryOrderRepository).Assembly;

	[Fact]
	public void Domain_ShouldNotDependOnApplicationOrInfrustructure()
	{
		var domainTypes = Types.InAssembly(DomainAssembly);

		//Act 
		var result = domainTypes
			.ShouldNot()
			.HaveDependencyOnAny(
			  ApplicationAssembly.GetName().Name,
			  InfrastructureAssembly.GetName().Name)
			.GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }

	[Fact]
    public void Application_ShouldNotDependOnInfrustructure()
    {
        var applicationTypes = Types.InAssembly(ApplicationAssembly);
        //Act 
        var result = applicationTypes
            .ShouldNot()
            .HaveDependencyOnAny(
              InfrastructureAssembly.GetName().Name)
            .GetResult();
        //Assert
        result.IsSuccessful.Should().BeTrue();
    }


    [Fact]
    public void Interfaces_ShouldStartWithI()
    {
        //Arrange
        var interfaces = Types.InAssemblies(new[]
        {
          DomainAssembly,
          ApplicationAssembly,
          InfrastructureAssembly

        })
         .That()
         .AreInterfaces();

        //Act
        var result = interfaces
            .Should()
            .HaveNameStartingWith("I")
            .GetResult();

        //Assert
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Types_WhenImplementingIOrderRepository_ShouldEndWithRepository()
    {
        //Arrange 
        var repository = Types.InAssemblies(new[]
        {
          DomainAssembly,
          ApplicationAssembly,
          InfrastructureAssembly
        })
        .That()
        .ImplementInterface(typeof(IOrderRepository));

        //Act
        var result = repository
            .Should()
            .HaveNameEndingWith("Repository")
            .GetResult();
         
        //Assert 
        result.IsSuccessful.Should().BeTrue();

    }
}
