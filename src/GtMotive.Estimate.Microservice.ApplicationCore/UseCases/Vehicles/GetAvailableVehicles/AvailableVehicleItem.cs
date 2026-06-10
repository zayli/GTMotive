using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.GetAvailableVehicles
{
    /// <summary>
    /// Represents a single available vehicle in the output.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="AvailableVehicleItem"/> class.
    /// </remarks>
    /// <param name="id">The vehicle identifier.</param>
    /// <param name="brand">The vehicle brand.</param>
    /// <param name="model">The vehicle model.</param>
    /// <param name="licensePlate">The vehicle license plate.</param>
    public sealed class AvailableVehicleItem(Guid id, string brand, string model, string licensePlate)
    {
        /// <summary>Gets the vehicle identifier.</summary>
        public Guid Id { get; } = id;

        /// <summary>Gets the vehicle brand.</summary>
        public string Brand { get; } = brand;

        /// <summary>Gets the vehicle model.</summary>
        public string Model { get; } = model;

        /// <summary>Gets the vehicle license plate.</summary>
        public string LicensePlate { get; } = licensePlate;
    }
}
