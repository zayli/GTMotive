using System;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.CreateVehicle;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.GetAvailableVehicles;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Specs
{
    public class CreateAndListVehiclesTests(MongoFixture fixture) : IClassFixture<MongoFixture>
    {
        private readonly MongoFixture fixture = fixture;

        [Fact]
        public async Task CreateVehicle_ThenListAvailable_ShouldReturnThePersistedVehicle()
        {
            // Arrange
            var createPresenter = new TestCreateVehiclePresenter();
            var listPresenter = new TestGetAvailableVehiclesPresenter();
            var logger = Mock.Of<IAppLogger<CreateVehicleUseCase>>();
            var listLogger = Mock.Of<IAppLogger<GetAvailableVehiclesUseCase>>();

            var createUseCase = new CreateVehicleUseCase(
                fixture.VehicleRepository,
                fixture.UnitOfWork,
                createPresenter,
                logger);

            var listUseCase = new GetAvailableVehiclesUseCase(
                fixture.VehicleRepository,
                listPresenter,
                listLogger);

            // Act
            await createUseCase.Execute(new CreateVehicleInput(
                "Toyota",
                "Corolla",
                "TST-001",
                DateTime.UtcNow.AddYears(-1)));

            await listUseCase.Execute(new GetAvailableVehiclesInput());

            // Assert
            createPresenter.Output.Should().NotBeNull();
            createPresenter.Output!.LicensePlate.Should().Be("TST-001");

            listPresenter.Output.Should().NotBeNull();
            listPresenter.Output!.Vehicles.Should().Contain(v => v.Id == createPresenter.Output.Id);
        }

        private sealed class TestCreateVehiclePresenter : ICreateVehicleOutputPort
        {
            public CreateVehicleOutput Output { get; private set; }

            public void StandardHandle(CreateVehicleOutput response) => Output = response;
        }

        private sealed class TestGetAvailableVehiclesPresenter : IGetAvailableVehiclesOutputPort
        {
            public GetAvailableVehiclesOutput Output { get; private set; }

            public void StandardHandle(GetAvailableVehiclesOutput response) => Output = response;
        }
    }
}
