using System;

namespace GtMotive.Estimate.Microservice.Domain.ValueObjects
{
    /// <summary>
    /// Represents the manufacturing date of a vehicle.
    /// Enforces the business rule that vehicles cannot be older than 5 years.
    /// </summary>
    public sealed class ManufacturingDate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManufacturingDate"/> class.
        /// </summary>
        /// <param name="value">The manufacturing date of the vehicle.</param>
        public ManufacturingDate(DateTime value)
        {
            if (value > DateTime.UtcNow)
            {
                throw new DomainException("Manufacturing date cannot be in the future.");
            }

            Value = value;
        }

        /// <summary>
        /// Gets the manufacturing date value.
        /// </summary>
        public DateTime Value { get; }
    }
}
