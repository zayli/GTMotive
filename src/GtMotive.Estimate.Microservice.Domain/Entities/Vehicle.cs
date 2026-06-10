using System;
using GtMotive.Estimate.Microservice.Domain.Enums;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.Entities
{
    /// <summary>
    /// Represents a vehicle in the rental fleet.
    /// </summary>
    public sealed class Vehicle
    {
        /// <summary>
        /// Maximum age, in years, a vehicle can have to be part of the fleet.
        /// </summary>
        public const int MaxFleetAgeInYears = 5;

        private Vehicle()
        {
        }

        /// <summary>
        /// Gets the unique identifier of the vehicle.
        /// </summary>
        public VehicleId Id { get; private set; }

        /// <summary>
        /// Gets the brand of the vehicle.
        /// </summary>
        public string Brand { get; private set; }

        /// <summary>
        /// Gets the model of the vehicle.
        /// </summary>
        public string Model { get; private set; }

        /// <summary>
        /// Gets the license plate of the vehicle.
        /// </summary>
        public LicensePlate LicensePlate { get; private set; }

        /// <summary>
        /// Gets the manufacturing date of the vehicle.
        /// </summary>
        public ManufacturingDate ManufacturingDate { get; private set; }

        /// <summary>
        /// Gets the current rental status of the vehicle.
        /// </summary>
        public VehicleStatus Status { get; private set; }

        /// <summary>
        /// Gets the identifier of the customer who has rented the vehicle, or null if available.
        /// </summary>
        public CustomerId? RentedByCustomerId { get; private set; }

        /// <summary>
        /// Creates a new vehicle and adds it to the fleet.
        /// </summary>
        /// <param name="brand">The brand of the vehicle.</param>
        /// <param name="model">The model of the vehicle.</param>
        /// <param name="licensePlate">The license plate of the vehicle.</param>
        /// <param name="manufacturingDate">The manufacturing date of the vehicle.</param>
        /// <returns>A new <see cref="Vehicle"/> instance.</returns>
        public static Vehicle Create(string brand, string model, LicensePlate licensePlate, ManufacturingDate manufacturingDate)
        {
            ValidateArguments(brand, model, manufacturingDate);

            return new Vehicle
            {
                Id = new VehicleId(Guid.NewGuid()),
                Brand = brand,
                Model = model,
                LicensePlate = licensePlate,
                ManufacturingDate = manufacturingDate,
                Status = VehicleStatus.Available
            };
        }

        /// <summary>
        /// Reconstructs a vehicle from its persisted state without applying admission rules.
        /// </summary>
        /// <param name="id">The unique identifier of the vehicle.</param>
        /// <param name="brand">The brand of the vehicle.</param>
        /// <param name="model">The model of the vehicle.</param>
        /// <param name="licensePlate">The license plate of the vehicle.</param>
        /// <param name="manufacturingDate">The manufacturing date of the vehicle.</param>
        /// <param name="status">The current rental status of the vehicle.</param>
        /// <param name="rentedByCustomerId">The identifier of the customer who rented the vehicle, if any.</param>
        /// <returns>A rehydrated <see cref="Vehicle"/> instance.</returns>
        public static Vehicle Rehydrate(
            VehicleId id,
            string brand,
            string model,
            LicensePlate licensePlate,
            ManufacturingDate manufacturingDate,
            VehicleStatus status,
            CustomerId? rentedByCustomerId)
        {
            return new Vehicle
            {
                Id = id,
                Brand = brand,
                Model = model,
                LicensePlate = licensePlate,
                ManufacturingDate = manufacturingDate,
                Status = status,
                RentedByCustomerId = rentedByCustomerId
            };
        }

        /// <summary>
        /// Determines whether the vehicle exceeds the maximum age allowed in the fleet.
        /// </summary>
        /// <returns>True if the vehicle is older than the fleet age limit; otherwise false.</returns>
        public bool ExceedsFleetAgeLimit()
        {
            return IsOlderThanFleetLimit(ManufacturingDate.Value);
        }

        /// <summary>
        /// Marks the vehicle as rented by the specified customer.
        /// </summary>
        /// <param name="customerId">The identifier of the customer renting the vehicle.</param>
        public void Rent(CustomerId customerId)
        {
            if (Status is VehicleStatus.Rented)
            {
                throw new DomainException("Vehicle is already rented.");
            }

            Status = VehicleStatus.Rented;
            RentedByCustomerId = customerId;
        }

        /// <summary>
        /// Marks the vehicle as returned and available for rental.
        /// </summary>
        public void Return()
        {
            if (Status is VehicleStatus.Available)
            {
                throw new DomainException("Vehicle is not rented.");
            }

            Status = VehicleStatus.Available;
            RentedByCustomerId = null;
        }

        private static bool IsOlderThanFleetLimit(DateTime manufacturingDate)
        {
            return manufacturingDate < DateTime.UtcNow.AddYears(-MaxFleetAgeInYears);
        }

        private static void ValidateArguments(string brand, string model, ManufacturingDate manufacturingDate)
        {
            if (string.IsNullOrWhiteSpace(brand))
            {
                throw new DomainException("Brand cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(model))
            {
                throw new DomainException("Model cannot be empty.");
            }

            if (manufacturingDate is null)
            {
                throw new DomainException("Manufacturing date is required.");
            }

            if (IsOlderThanFleetLimit(manufacturingDate.Value))
            {
                throw new DomainException("Vehicle cannot be older than 5 years to join the fleet.");
            }
        }
    }
}
