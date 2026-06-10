using System;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Customers.CreateCustomer;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.CreateVehicle;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.RentVehicle;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;
using GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Specs
{
    public class RentVehicleFlowTests(MongoFixture fixture) : IClassFixture<MongoFixture>
    {
        private readonly MongoFixture fixture = fixture;

        [Fact]
        public async Task RentVehicle_AfterCreating_ShouldMarkVehicleAsRented()
        {
            // Arrange: seed a customer + vehicle through their use cases.
            var customerOutput = await CreateCustomerAsync("Alice", "12345678A");
            var vehicleOutput = await CreateVehicleAsync("Toyota", "Yaris", "RNT-001");

            var rentPresenter = new TestRentVehiclePresenter();
            var rentUseCase = new RentVehicleUseCase(
                fixture.VehicleRepository,
                fixture.CustomerRepository,
                fixture.UnitOfWork,
                rentPresenter,
                Mock.Of<IAppLogger<RentVehicleUseCase>>());

            // Act
            await rentUseCase.Execute(new RentVehicleInput(vehicleOutput.Id, customerOutput.Id));

            // Assert: presenter received the standard output and Mongo state reflects the rent.
            rentPresenter.Output.Should().NotBeNull();
            rentPresenter.Output!.VehicleId.Should().Be(vehicleOutput.Id);

            var rehydrated = await fixture.VehicleRepository.GetByIdAsync(new VehicleId(vehicleOutput.Id));
            rehydrated!.RentedByCustomerId!.Value.Value.Should().Be(customerOutput.Id);
        }

        [Fact]
        public async Task RentVehicle_WhenCustomerAlreadyHasRental_ShouldThrowDomainException()
        {
            // Arrange: a customer that already rented one vehicle.
            var customerOutput = await CreateCustomerAsync("Bob", "87654321B");
            var firstVehicle = await CreateVehicleAsync("Ford", "Focus", "RNT-002");
            var secondVehicle = await CreateVehicleAsync("Ford", "Fiesta", "RNT-003");

            var rentUseCase = new RentVehicleUseCase(
                fixture.VehicleRepository,
                fixture.CustomerRepository,
                fixture.UnitOfWork,
                new TestRentVehiclePresenter(),
                Mock.Of<IAppLogger<RentVehicleUseCase>>());

            await rentUseCase.Execute(new RentVehicleInput(firstVehicle.Id, customerOutput.Id));

            // Act
            Func<Task> act = async () =>
                await rentUseCase.Execute(new RentVehicleInput(secondVehicle.Id, customerOutput.Id));

            // Assert
            await act.Should()
                .ThrowAsync<DomainException>()
                .WithMessage("*already has an active rental*");
        }

        private async Task<CreateCustomerOutput> CreateCustomerAsync(string name, string documentId)
        {
            var presenter = new TestCreateCustomerPresenter();
            var useCase = new CreateCustomerUseCase(
                fixture.CustomerRepository,
                fixture.UnitOfWork,
                presenter,
                Mock.Of<IAppLogger<CreateCustomerUseCase>>());
            await useCase.Execute(new CreateCustomerInput(name, documentId));
            return presenter.Output;
        }

        private async Task<CreateVehicleOutput> CreateVehicleAsync(string brand, string model, string plate)
        {
            var presenter = new TestCreateVehiclePresenter();
            var useCase = new CreateVehicleUseCase(
                fixture.VehicleRepository,
                fixture.UnitOfWork,
                presenter,
                Mock.Of<IAppLogger<CreateVehicleUseCase>>());
            await useCase.Execute(new CreateVehicleInput(brand, model, plate, DateTime.UtcNow.AddYears(-1)));
            return presenter.Output;
        }

        private sealed class TestCreateCustomerPresenter : ICreateCustomerOutputPort
        {
            public CreateCustomerOutput Output { get; private set; }

            public void StandardHandle(CreateCustomerOutput response) => Output = response;
        }

        private sealed class TestCreateVehiclePresenter : ICreateVehicleOutputPort
        {
            public CreateVehicleOutput Output { get; private set; }

            public void StandardHandle(CreateVehicleOutput response) => Output = response;
        }

        private sealed class TestRentVehiclePresenter : IRentVehicleOutputPort
        {
            public RentVehicleOutput Output { get; private set; }

            public string NotFoundMessage { get; private set; }

            public void StandardHandle(RentVehicleOutput response) => Output = response;

            public void NotFoundHandle(string message) => NotFoundMessage = message;
        }
    }
}
