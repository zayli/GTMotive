using System;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.CreateVehicle;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.ApplicationCore
{
    public class CreateVehicleUseCaseTests
    {
        private static readonly DateTime UtcNow = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly TimeProvider Time = new FixedTimeProvider(UtcNow);

        [Fact]
        public async Task Execute_WithValidInput_ShouldPersistAndCallStandardOutput()
        {
            var vehicleRepo = new Mock<IVehicleRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var outputPort = new Mock<ICreateVehicleOutputPort>();
            var logger = Mock.Of<IAppLogger<CreateVehicleUseCase>>();

            var useCase = new CreateVehicleUseCase(vehicleRepo.Object, unitOfWork.Object, outputPort.Object, logger, Time);
            var input = new CreateVehicleInput("Toyota", "Corolla", "ABC-123", UtcNow.AddYears(-1));

            await useCase.Execute(input);

            vehicleRepo.Verify(r => r.AddAsync(It.IsAny<Vehicle>()), Times.Once);
            unitOfWork.Verify(u => u.Save(), Times.Once);
            outputPort.Verify(o => o.StandardHandle(It.Is<CreateVehicleOutput>(x => x.LicensePlate == "ABC-123")), Times.Once);
        }

        [Fact]
        public async Task Execute_WithVehicleOlderThanFleetLimit_ShouldThrowDomainException()
        {
            var vehicleRepo = new Mock<IVehicleRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var outputPort = new Mock<ICreateVehicleOutputPort>();
            var logger = Mock.Of<IAppLogger<CreateVehicleUseCase>>();

            var useCase = new CreateVehicleUseCase(vehicleRepo.Object, unitOfWork.Object, outputPort.Object, logger, Time);
            var input = new CreateVehicleInput("Toyota", "Corolla", "OLD-001", UtcNow.AddYears(-Vehicle.MaxFleetAgeInYears - 1));

            Func<Task> act = () => useCase.Execute(input);

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
                Mock.Of<IAppLogger<CreateVehicleUseCase>>(),
                Time);

            Func<Task> act = () => useCase.Execute(null);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Execute_WhenLicensePlateAlreadyExists_ShouldThrowDomainException()
        {
            var vehicleRepo = new Mock<IVehicleRepository>();
            vehicleRepo
                .Setup(r => r.ExistsByLicensePlateAsync(It.IsAny<LicensePlate>()))
                .ReturnsAsync(true);

            var useCase = new CreateVehicleUseCase(
                vehicleRepo.Object,
                Mock.Of<IUnitOfWork>(),
                Mock.Of<ICreateVehicleOutputPort>(),
                Mock.Of<IAppLogger<CreateVehicleUseCase>>(),
                Time);

            var input = new CreateVehicleInput("Toyota", "Corolla", "DUP-001", UtcNow.AddYears(-1));

            Func<Task> act = () => useCase.Execute(input);

            await act.Should()
                .ThrowAsync<DomainException>()
                .WithMessage("*license plate already exists*");
            vehicleRepo.Verify(r => r.AddAsync(It.IsAny<Vehicle>()), Times.Never);
        }

        private sealed class FixedTimeProvider(DateTime utcNow) : TimeProvider
        {
            public override DateTimeOffset GetUtcNow() => new(utcNow, TimeSpan.Zero);
        }
    }
}
