using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.RentVehicle
{
    /// <summary>
    /// Output message for the rent vehicle use case.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="RentVehicleOutput"/> class.
    /// </remarks>
    /// <param name="vehicleId">The identifier of the rented vehicle.</param>
    /// <param name="customerId">The identifier of the customer.</param>
    public sealed class RentVehicleOutput(Guid vehicleId, Guid customerId) : IUseCaseOutput
    {
        /// <summary>Gets the identifier of the rented vehicle.</summary>
        public Guid VehicleId { get; } = vehicleId;

        /// <summary>Gets the identifier of the customer.</summary>
        public Guid CustomerId { get; } = customerId;
    }
}
