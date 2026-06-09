namespace GtMotive.Estimate.Microservice.Domain.Enums
{
    /// <summary>
    /// Represents the availability status of a vehicle in the fleet.
    /// </summary>
    public enum VehicleStatus
    {
        /// <summary>
        /// The vehicle is available for rental.
        /// </summary>
        Available = 0,

        /// <summary>
        /// The vehicle is currently rented.
        /// </summary>
        Rented = 1
    }
}
