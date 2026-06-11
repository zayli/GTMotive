using System;
using FluentAssertions;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Enums;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.Domain
{
    public class VehicleTests
    {
        private static readonly DateTime UtcNow = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        [Fact]
        public void Create_WhenVehicleIsOlderThanFleetLimit_ShouldThrowDomainException()
        {
            var tooOld = new ManufacturingDate(UtcNow.AddYears(-Vehicle.MaxFleetAgeInYears).AddDays(-1));
            var plate = new LicensePlate("OLD-001");

            Action act = () => Vehicle.Create("Toyota", "Corolla", plate, tooOld, UtcNow);

            act.Should()
                .Throw<DomainException>()
                .WithMessage("*5 years*");
        }

        [Fact]
        public void Create_WithValidArguments_ShouldStartAsAvailable()
        {
            var date = new ManufacturingDate(UtcNow.AddYears(-1));
            var plate = new LicensePlate("ABC-123");

            var vehicle = Vehicle.Create("Toyota", "Corolla", plate, date, UtcNow);

            vehicle.Status.Should().Be(VehicleStatus.Available);
            vehicle.RentedByCustomerId.Should().BeNull();
            vehicle.Brand.Should().Be("Toyota");
        }

        [Fact]
        public void Rent_WhenVehicleIsAvailable_ShouldChangeStatusToRented()
        {
            var vehicle = BuildAvailableVehicle();
            var customerId = new CustomerId(Guid.NewGuid());

            vehicle.Rent(customerId);

            vehicle.Status.Should().Be(VehicleStatus.Rented);
            vehicle.RentedByCustomerId.Should().Be(customerId);
        }

        [Fact]
        public void Rent_WhenVehicleIsAlreadyRented_ShouldThrowDomainException()
        {
            var vehicle = BuildAvailableVehicle();
            vehicle.Rent(new CustomerId(Guid.NewGuid()));

            Action act = () => vehicle.Rent(new CustomerId(Guid.NewGuid()));

            act.Should()
                .Throw<DomainException>()
                .WithMessage("*already rented*");
        }

        [Fact]
        public void Return_WhenVehicleIsRented_ShouldChangeStatusToAvailable()
        {
            var vehicle = BuildAvailableVehicle();
            vehicle.Rent(new CustomerId(Guid.NewGuid()));

            vehicle.Return();

            vehicle.Status.Should().Be(VehicleStatus.Available);
            vehicle.RentedByCustomerId.Should().BeNull();
        }

        [Fact]
        public void Return_WhenVehicleIsAvailable_ShouldThrowDomainException()
        {
            var vehicle = BuildAvailableVehicle();

            Action act = vehicle.Return;

            act.Should()
                .Throw<DomainException>()
                .WithMessage("*not rented*");
        }

        private static Vehicle BuildAvailableVehicle() => Vehicle.Create(
            "Toyota",
            "Yaris",
            new LicensePlate("AVL-001"),
            new ManufacturingDate(UtcNow.AddYears(-1)),
            UtcNow);
    }
}
