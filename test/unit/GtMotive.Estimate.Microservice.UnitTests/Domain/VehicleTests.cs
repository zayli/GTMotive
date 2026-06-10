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
        [Fact]
        public void Create_WhenVehicleIsOlderThanFleetLimit_ShouldThrowDomainException()
        {
            // Arrange: build a vehicle older than 5 years (the fleet admission rule).
            var tooOld = new ManufacturingDate(DateTime.UtcNow.AddYears(-Vehicle.MaxFleetAgeInYears).AddDays(-1));
            var plate = new LicensePlate("OLD-001");

            // Act
            Action act = () => Vehicle.Create("Toyota", "Corolla", plate, tooOld);

            // Assert
            act.Should()
                .Throw<DomainException>()
                .WithMessage("*5 years*");
        }

        [Fact]
        public void Create_WithValidArguments_ShouldStartAsAvailable()
        {
            // Arrange
            var date = new ManufacturingDate(DateTime.UtcNow.AddYears(-1));
            var plate = new LicensePlate("ABC-123");

            // Act
            var vehicle = Vehicle.Create("Toyota", "Corolla", plate, date);

            // Assert
            vehicle.Status.Should().Be(VehicleStatus.Available);
            vehicle.RentedByCustomerId.Should().BeNull();
            vehicle.Brand.Should().Be("Toyota");
        }
    }
}
