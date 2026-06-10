using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.ReturnVehicle
{
    /// <summary>
    /// Output message for the return vehicle use case.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ReturnVehicleOutput"/> class.
    /// </remarks>
    /// <param name="vehicleId">The identifier of the returned vehicle.</param>
    public sealed class ReturnVehicleOutput(Guid vehicleId) : IUseCaseOutput
    {
        /// <summary>Gets the identifier of the returned vehicle.</summary>
        public Guid VehicleId { get; } = vehicleId;
    }
}
