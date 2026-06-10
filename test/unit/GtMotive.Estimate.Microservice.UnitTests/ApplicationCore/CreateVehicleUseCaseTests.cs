using System;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.CreateVehicle;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.ApplicationCore
{
    public class CreateVehicleUseCaseTests
    {
        [Fact]
        public async Task Execute_WithValidInput_ShouldPersistAndCallStandardOutput()
        {
            // Arrange
            var vehicleRepo = new Mock<IVehicleRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var outputPort = new Mock<ICreateVehicleOutputPort>();
            var logger = Mock.Of<IAppLogger<CreateVehicleUseCase>>();

            var useCase = new CreateVehicleUseCase(vehicleRepo.Object, unitOfWork.Object, outputPort.Object, logger);
            var input = new CreateVehicleInput("Toyota", "Corolla", "ABC-123", DateTime.UtcNow.AddYears(-1));

            // Act
            await useCase.Execute(input);

            // Assert
            vehicleRepo.Verify(r => r.AddAsync(It.IsAny<Vehicle>()), Times.Once);
            unitOfWork.Verify(u => u.Save(), Times.Once);
            outputPort.Verify(o => o.StandardHandle(It.Is<CreateVehicleOutput>(x => x.LicensePlate == "ABC-123")), Times.Once);
        }

        [Fact]
        public async Task Execute_WithVehicleOlderThanFleetLimit_ShouldThrowDomainException()
        {
            // Arrange
            var vehicleRepo = new Mock<IVehicleRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var outputPort = new Mock<ICreateVehicleOutputPort>();
            var logger = Mock.Of<IAppLogger<CreateVehicleUseCase>>();

            var useCase = new CreateVehicleUseCase(vehicleRepo.Object, unitOfWork.Object, outputPort.Object, logger);
            var input = new CreateVehicleInput("Toyota", "Corolla", "OLD-001", DateTime.UtcNow.AddYears(-Vehicle.MaxFleetAgeInYears - 1));

            // Act
            Func<Task> act = () => useCase.Execute(input);

            // Assert
            await act.Should().ThrowAsync<DomainException>();
            vehicleRepo.Verify(r => r.AddAsync(It.IsAny<Vehicle>()), Times.Never);
        }

        [Fact]
        public async Task Execute_WithNullInput_ShouldThrowArgumentNullException()
        {
            var useCase = new CreateVehicleUseCase(
                Mock.Of<IVehicleRepository>(),
                Mock.Of<IUnitOfWork>(),
                Mock.Of<ICreateVehicleOutputPort>(),
                Mock.Of<IAppLogger<CreateVehicleUseCase>>());

            Func<Task> act = () => useCase.Execute(null);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
