using System;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.ReturnVehicle;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Enums;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.ApplicationCore
{
    public class ReturnVehicleUseCaseTests
    {
        [Fact]
        public async Task Execute_WhenVehicleIsRented_ShouldMarkAvailableAndCallStandardOutput()
        {
            // Arrange
            var rentedVehicle = Vehicle.Rehydrate(
                new VehicleId(Guid.NewGuid()),
                "Toyota",
                "Yaris",
                new LicensePlate("RNT-001"),
                new ManufacturingDate(DateTime.UtcNow.AddYears(-1)),
                VehicleStatus.Rented,
                new CustomerId(Guid.NewGuid()));

            var vehicleRepo = new Mock<IVehicleRepository>();
            vehicleRepo.Setup(r => r.GetByIdAsync(It.IsAny<VehicleId>())).ReturnsAsync(rentedVehicle);
            var unitOfWork = new Mock<IUnitOfWork>();
            var outputPort = new Mock<IReturnVehicleOutputPort>();

            var useCase = new ReturnVehicleUseCase(
                vehicleRepo.Object,
                unitOfWork.Object,
                outputPort.Object,
                Mock.Of<IAppLogger<ReturnVehicleUseCase>>());

            // Act
            await useCase.Execute(new ReturnVehicleInput(rentedVehicle.Id.Value));

            // Assert
            rentedVehicle.Status.Should().Be(VehicleStatus.Available);
            vehicleRepo.Verify(r => r.UpdateAsync(rentedVehicle), Times.Once);
            outputPort.Verify(o => o.StandardHandle(It.IsAny<ReturnVehicleOutput>()), Times.Once);
            outputPort.Verify(o => o.NotFoundHandle(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Execute_WhenVehicleDoesNotExist_ShouldCallNotFoundOutput()
        {
            // Arrange
            var vehicleRepo = new Mock<IVehicleRepository>();
            vehicleRepo.Setup(r => r.GetByIdAsync(It.IsAny<VehicleId>())).ReturnsAsync((Vehicle)null);
            var unitOfWork = new Mock<IUnitOfWork>();
            var outputPort = new Mock<IReturnVehicleOutputPort>();

            var useCase = new ReturnVehicleUseCase(
                vehicleRepo.Object,
                unitOfWork.Object,
                outputPort.Object,
                Mock.Of<IAppLogger<ReturnVehicleUseCase>>());

            // Act
            await useCase.Execute(new ReturnVehicleInput(Guid.NewGuid()));

            // Assert
            outputPort.Verify(o => o.NotFoundHandle("Vehicle not found."), Times.Once);
            vehicleRepo.Verify(r => r.UpdateAsync(It.IsAny<Vehicle>()), Times.Never);
            unitOfWork.Verify(u => u.Save(), Times.Never);
        }
    }
}
