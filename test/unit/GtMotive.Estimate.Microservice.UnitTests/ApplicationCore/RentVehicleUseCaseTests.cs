using System;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.RentVehicle;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Enums;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.ApplicationCore
{
    public class RentVehicleUseCaseTests
    {
        [Fact]
        public async Task Execute_WhenVehicleDoesNotExist_ShouldInvokeNotFoundOutput()
        {
            var vehicleRepo = new Mock<IVehicleRepository>();
            var customerRepo = new Mock<ICustomerRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var outputPort = new Mock<IRentVehicleOutputPort>();
            var logger = new Mock<IAppLogger<RentVehicleUseCase>>();

            vehicleRepo
                .Setup(r => r.GetByIdAsync(It.IsAny<VehicleId>()))
                .ReturnsAsync((Vehicle)null);

            var useCase = new RentVehicleUseCase(
                vehicleRepo.Object,
                customerRepo.Object,
                unitOfWork.Object,
                outputPort.Object,
                logger.Object);

            var input = new RentVehicleInput(Guid.NewGuid(), Guid.NewGuid());

            await useCase.Execute(input);

            outputPort.Verify(o => o.NotFoundHandle("Vehicle not found."), Times.Once);
            outputPort.Verify(o => o.StandardHandle(It.IsAny<RentVehicleOutput>()), Times.Never);
            customerRepo.Verify(r => r.GetByIdAsync(It.IsAny<CustomerId>()), Times.Never);
            unitOfWork.Verify(u => u.Save(), Times.Never);
        }

        [Fact]
        public async Task Execute_WhenCustomerDoesNotExist_ShouldInvokeNotFoundOutput()
        {
            var vehicleRepo = new Mock<IVehicleRepository>();
            vehicleRepo.Setup(r => r.GetByIdAsync(It.IsAny<VehicleId>())).ReturnsAsync(BuildAvailableVehicle());
            var customerRepo = new Mock<ICustomerRepository>();
            customerRepo.Setup(r => r.GetByIdAsync(It.IsAny<CustomerId>())).ReturnsAsync((Customer)null);

            var outputPort = new Mock<IRentVehicleOutputPort>();
            var unitOfWork = new Mock<IUnitOfWork>();

            var useCase = new RentVehicleUseCase(
                vehicleRepo.Object,
                customerRepo.Object,
                unitOfWork.Object,
                outputPort.Object,
                Mock.Of<IAppLogger<RentVehicleUseCase>>());

            await useCase.Execute(new RentVehicleInput(Guid.NewGuid(), Guid.NewGuid()));

            outputPort.Verify(o => o.NotFoundHandle("Customer not found."), Times.Once);
            unitOfWork.Verify(u => u.Save(), Times.Never);
        }

        [Fact]
        public async Task Execute_WhenCustomerAlreadyHasActiveRental_ShouldThrowDomainException()
        {
            var vehicleRepo = new Mock<IVehicleRepository>();
            vehicleRepo.Setup(r => r.GetByIdAsync(It.IsAny<VehicleId>())).ReturnsAsync(BuildAvailableVehicle());
            vehicleRepo.Setup(r => r.HasActiveRentaAsync(It.IsAny<CustomerId>())).ReturnsAsync(true);

            var customerRepo = new Mock<ICustomerRepository>();
            customerRepo.Setup(r => r.GetByIdAsync(It.IsAny<CustomerId>())).ReturnsAsync(Customer.Create("Alice", "12345678A"));

            var useCase = new RentVehicleUseCase(
                vehicleRepo.Object,
                customerRepo.Object,
                Mock.Of<IUnitOfWork>(),
                Mock.Of<IRentVehicleOutputPort>(),
                Mock.Of<IAppLogger<RentVehicleUseCase>>());

            Func<Task> act = () => useCase.Execute(new RentVehicleInput(Guid.NewGuid(), Guid.NewGuid()));

            await act.Should()
                .ThrowAsync<DomainException>()
                .WithMessage("*already has an active rental*");
        }

        private static Vehicle BuildAvailableVehicle() => Vehicle.Rehydrate(
            new VehicleId(Guid.NewGuid()),
            "Toyota",
            "Yaris",
            new LicensePlate("AVL-001"),
            new ManufacturingDate(DateTime.UtcNow.AddYears(-1)),
            VehicleStatus.Available,
            null);
    }
}
