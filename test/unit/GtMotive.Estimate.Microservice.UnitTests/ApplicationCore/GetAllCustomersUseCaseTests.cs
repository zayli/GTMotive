using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Customers.GetAllCustomers;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.ApplicationCore
{
    public class GetAllCustomersUseCaseTests
    {
        [Fact]
        public async Task Execute_ShouldReturnAllCustomersFromRepository()
        {
            // Arrange
            var alice = Customer.Create("Alice", "12345678A");
            var bob = Customer.Create("Bob", "87654321B");
            var customerRepo = new Mock<ICustomerRepository>();
            customerRepo.Setup(r => r.GetAllAsync()).ReturnsAsync([alice, bob]);

            var outputPort = new TestPresenter();
            var useCase = new GetAllCustomersUseCase(
                customerRepo.Object,
                outputPort,
                Mock.Of<IAppLogger<GetAllCustomersUseCase>>());

            // Act
            await useCase.Execute(new GetAllCustomersInput());

            // Assert
            outputPort.Captured.Should().NotBeNull();
            outputPort.Captured!.Customers.Should().HaveCount(2);
            outputPort.Captured.Customers.Select(c => c.Name).Should().Contain(["Alice", "Bob"]);
        }

        [Fact]
        public async Task Execute_WhenRepositoryIsEmpty_ShouldReturnEmptyOutput()
        {
            var customerRepo = new Mock<ICustomerRepository>();
            customerRepo.Setup(r => r.GetAllAsync()).ReturnsAsync([]);

            var outputPort = new TestPresenter();
            var useCase = new GetAllCustomersUseCase(
                customerRepo.Object,
                outputPort,
                Mock.Of<IAppLogger<GetAllCustomersUseCase>>());

            await useCase.Execute(new GetAllCustomersInput());

            outputPort.Captured!.Customers.Should().BeEmpty();
        }

        private sealed class TestPresenter : IGetAllCustomersOutputPort
        {
            public GetAllCustomersOutput Captured { get; private set; }

            public void StandardHandle(GetAllCustomersOutput response) => Captured = response;
        }
    }
}
