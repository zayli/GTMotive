using System.Collections.Generic;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.GetAvailableVehicles
{
    /// <summary>
    /// Output message for the get available vehicles use case.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="GetAvailableVehiclesOutput"/> class.
    /// </remarks>
    /// <param name="vehicles">The collection of available vehicles.</param>
    public sealed class GetAvailableVehiclesOutput(IEnumerable<AvailableVehicleItem> vehicles) : IUseCaseOutput
    {
        /// <summary>Gets the collection of available vehicles.</summary>
        public IEnumerable<AvailableVehicleItem> Vehicles { get; } = vehicles;
    }
}
