using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.CreateVehicle
{
    /// <summary>
    /// Output message for the create vehicle use case.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CreateVehicleOutput"/> class.
    /// </remarks>
    /// <param name="id">The unique identifier of the created vehicle.</param>
    /// <param name="brand">The brand of the vehicle.</param>
    /// <param name="model">The model of the vehicle.</param>
    /// <param name="licensePlate">The license plate of the vehicle.</param>
    public sealed class CreateVehicleOutput(Guid id, string brand, string model, string licensePlate) : IUseCaseOutput
    {
        /// <summary>Gets the unique identifier of the created vehicle.</summary>
        public Guid Id { get; } = id;

        /// <summary>Gets the brand of the vehicle.</summary>
        public string Brand { get; } = brand;

        /// <summary>Gets the model of the vehicle.</summary>
        public string Model { get; } = model;

        /// <summary>Gets the license plate of the vehicle.</summary>
        public string LicensePlate { get; } = licensePlate;
    }
}
