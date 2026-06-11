using System;
using FluentAssertions;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.Domain
{
    public class ValueObjectTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void LicensePlate_WithBlankValue_ShouldThrowDomainException(string value)
        {
            Action act = () => _ = new LicensePlate(value);

            act.Should()
                .Throw<DomainException>()
                .WithMessage("*empty*");
        }

        [Fact]
        public void LicensePlate_WithLowercaseValue_ShouldNormalizeToUppercase()
        {
            var plate = new LicensePlate("abc-123 ");

            plate.Value.Should().Be("ABC-123");
        }

        [Fact]
        public void ManufacturingDate_WithFutureDate_ShouldThrowDomainException()
        {
            var future = DateTime.UtcNow.AddDays(1);

            Action act = () => _ = new ManufacturingDate(future);

            act.Should()
                .Throw<DomainException>()
                .WithMessage("*future*");
        }
    }
}
