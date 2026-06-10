using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.ReturnVehicle
{
    /// <summary>
    /// Input message for the return vehicle use case.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ReturnVehicleInput"/> class.
    /// </remarks>
    /// <param name="vehicleId">The identifier of the vehicle to return.</param>
    public sealed class ReturnVehicleInput(Guid vehicleId) : IUseCaseInput
    {
        /// <summary>Gets the identifier of the vehicle to return.</summary>
        public Guid VehicleId { get; } = vehicleId;
    }
}
