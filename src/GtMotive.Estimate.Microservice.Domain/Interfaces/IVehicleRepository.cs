#nullable enable
using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.Interfaces
{
    /// <summary>
    /// Defines the repository contract for vehicle persistence operations.
    /// </summary>
    public interface IVehicleRepository
    {
        /// <summary>
        /// Adds a new vehicle to the repository.
        /// </summary>
        /// <param name="vehicle">The vehicle to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddAsync(Vehicle vehicle);

        /// <summary>
        /// Retrieves a vehicle by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the vehicle.</param>
        /// <returns>The vehicle if found; otherwise null.</returns>
        Task<Vehicle?> GetByIdAsync(VehicleId id);

        /// <summary>
        /// Retrieves all vehicles with Available status.
        /// </summary>
        /// <returns>A collection of available vehicles.</returns>
        Task<IEnumerable<Vehicle>> GetAvailableAsync();

        /// <summary>
        /// Updates an existing vehicle in the repository.
        /// </summary>
        /// <param name="vehicle">The vehicle with updated values.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAsync(Vehicle vehicle);

        /// <summary>
        /// Determines whether the specified customer currently has a rented vehicle.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer.</param>
        /// <returns>True if the customer has an active rental; otherwise false.</returns>
        Task<bool> HasActiveRentaAsync(CustomerId customerId);

        /// <summary>
        /// Determines whether a vehicle with the given license plate already exists.
        /// </summary>
        /// <param name="licensePlate">The license plate to look up.</param>
        /// <returns>True if a vehicle with that plate is already registered; otherwise false.</returns>
        Task<bool> ExistsByLicensePlateAsync(LicensePlate licensePlate);
    }
}
