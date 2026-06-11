using System;
using GtMotive.Estimate.Microservice.Domain.Enums;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.Entities
{
    /// <summary>A vehicle in the rental fleet.</summary>
    public sealed class Vehicle
    {
        /// <summary>Max age (in years) for a vehicle to belong to the fleet.</summary>
        public const int MaxFleetAgeInYears = 5;

        private Vehicle()
        {
        }

        /// <summary>Gets the vehicle identifier.</summary>
        public VehicleId Id { get; private set; }

        /// <summary>Gets the brand.</summary>
        public string Brand { get; private set; }

        /// <summary>Gets the model.</summary>
        public string Model { get; private set; }

        /// <summary>Gets the license plate.</summary>
        public LicensePlate LicensePlate { get; private set; }

        /// <summary>Gets the manufacturing date.</summary>
        public ManufacturingDate ManufacturingDate { get; private set; }

        /// <summary>Gets the current rental status.</summary>
        public VehicleStatus Status { get; private set; }

        /// <summary>Gets the renter id, or null if the vehicle is available.</summary>
        public CustomerId? RentedByCustomerId { get; private set; }

        /// <summary>Creates a new vehicle and admits it into the fleet.</summary>
        /// <param name="brand">Brand.</param>
        /// <param name="model">Model.</param>
        /// <param name="licensePlate">License plate.</param>
        /// <param name="manufacturingDate">Manufacturing date.</param>
        /// <param name="utcNow">Current UTC time (injected from the application layer).</param>
        /// <returns>The new vehicle.</returns>
        public static Vehicle Create(string brand, string model, LicensePlate licensePlate, ManufacturingDate manufacturingDate, DateTime utcNow)
        {
            ValidateArguments(brand, model, manufacturingDate, utcNow);

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

        /// <summary>Rebuilds a vehicle from persistence. Skips admission rules.</summary>
        /// <param name="id">Vehicle id.</param>
        /// <param name="brand">Brand.</param>
        /// <param name="model">Model.</param>
        /// <param name="licensePlate">License plate.</param>
        /// <param name="manufacturingDate">Manufacturing date.</param>
        /// <param name="status">Rental status.</param>
        /// <param name="rentedByCustomerId">Renter id, if any.</param>
        /// <returns>The rehydrated vehicle.</returns>
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

        /// <summary>True if the vehicle is older than the fleet age limit.</summary>
        /// <param name="utcNow">Current UTC time.</param>
        /// <returns>Whether the limit is exceeded.</returns>
        public bool ExceedsFleetAgeLimit(DateTime utcNow)
        {
            return IsOlderThanFleetLimit(ManufacturingDate.Value, utcNow);
        }

        /// <summary>Marks the vehicle as rented by the given customer.</summary>
        /// <param name="customerId">Renter id.</param>
        public void Rent(CustomerId customerId)
        {
            if (Status is VehicleStatus.Rented)
            {
                throw new DomainException("Vehicle is already rented.");
            }

            Status = VehicleStatus.Rented;
            RentedByCustomerId = customerId;
        }

        /// <summary>Marks the vehicle as returned and available again.</summary>
        public void Return()
        {
            if (Status is VehicleStatus.Available)
            {
                throw new DomainException("Vehicle is not rented.");
            }

            Status = VehicleStatus.Available;
            RentedByCustomerId = null;
        }

        private static bool IsOlderThanFleetLimit(DateTime manufacturingDate, DateTime utcNow)
        {
            return manufacturingDate < utcNow.AddYears(-MaxFleetAgeInYears);
        }

        private static void ValidateArguments(string brand, string model, ManufacturingDate manufacturingDate, DateTime utcNow)
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

            if (IsOlderThanFleetLimit(manufacturingDate.Value, utcNow))
            {
                throw new DomainException("Vehicle cannot be older than 5 years to join the fleet.");
            }
        }
    }
}
