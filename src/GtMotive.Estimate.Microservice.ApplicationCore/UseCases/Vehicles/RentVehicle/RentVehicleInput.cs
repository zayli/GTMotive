using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.RentVehicle
{
    /// <summary>
    /// Input message for the rent vehicle use case.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="RentVehicleInput"/> class.
    /// </remarks>
    /// <param name="vehicleId">The identifier of the vehicle to rent.</param>
    /// <param name="customerId">The identifier of the customer renting the vehicle.</param>
    public sealed class RentVehicleInput(Guid vehicleId, Guid customerId) : IUseCaseInput
    {
        /// <summary>Gets the identifier of the vehicle to rent.</summary>
        public Guid VehicleId { get; } = vehicleId;

        /// <summary>Gets the identifier of the customer renting the vehicle.</summary>
        public Guid CustomerId { get; } = customerId;
    }
}
