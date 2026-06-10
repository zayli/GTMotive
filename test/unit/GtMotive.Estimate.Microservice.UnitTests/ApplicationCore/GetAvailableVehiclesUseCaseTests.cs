using System;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.GetAvailableVehicles;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Enums;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.ApplicationCore
{
    public class GetAvailableVehiclesUseCaseTests
    {
        [Fact]
        public async Task Execute_ShouldReturnVehiclesFromRepository()
        {
            // Arrange
            var fresh = BuildVehicle(DateTime.UtcNow.AddYears(-2));
            var vehicleRepo = new Mock<IVehicleRepository>();
            vehicleRepo.Setup(r => r.GetAvailableAsync()).ReturnsAsync([fresh]);

            var outputPort = new TestPresenter();
            var useCase = new GetAvailableVehiclesUseCase(
                vehicleRepo.Object,
                outputPort,
                Mock.Of<IAppLogger<GetAvailableVehiclesUseCase>>());

            // Act
            await useCase.Execute(new GetAvailableVehiclesInput());

            // Assert
            outputPort.Captured.Should().NotBeNull();
            outputPort.Captured!.Vehicles.Should().ContainSingle();
        }

        [Fact]
        public async Task Execute_ShouldFilterOutVehiclesExceedingFleetAgeLimit()
        {
            // Arrange: a rehydrated vehicle older than 5 years (admission rule does not apply
            // when rehydrating, but the listing must hide it).
            var aged = Vehicle.Rehydrate(
                new VehicleId(Guid.NewGuid()),
                "Old",
                "Model",
                new LicensePlate("OLD-X"),
                new ManufacturingDate(DateTime.UtcNow.AddYears(-Vehicle.MaxFleetAgeInYears - 1)),
                VehicleStatus.Available,
                null);

            var vehicleRepo = new Mock<IVehicleRepository>();
            vehicleRepo.Setup(r => r.GetAvailableAsync()).ReturnsAsync([aged]);

            var outputPort = new TestPresenter();
            var useCase = new GetAvailableVehiclesUseCase(
                vehicleRepo.Object,
                outputPort,
                Mock.Of<IAppLogger<GetAvailableVehiclesUseCase>>());

            // Act
            await useCase.Execute(new GetAvailableVehiclesInput());

            // Assert
            outputPort.Captured!.Vehicles.Should().BeEmpty();
        }

        private static Vehicle BuildVehicle(DateTime manufacturingDate) => Vehicle.Create(
            "Toyota",
            "Yaris",
            new LicensePlate("AVL-001"),
            new ManufacturingDate(manufacturingDate));

        private sealed class TestPresenter : IGetAvailableVehiclesOutputPort
        {
            public GetAvailableVehiclesOutput Captured { get; private set; }

            public void StandardHandle(GetAvailableVehiclesOutput response) => Captured = response;
        }
    }
}
