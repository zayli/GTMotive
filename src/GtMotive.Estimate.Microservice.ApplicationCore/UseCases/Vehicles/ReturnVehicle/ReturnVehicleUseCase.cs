using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.ReturnVehicle
{
    /// <summary>
    /// Use case that returns a rented vehicle, making it available again.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ReturnVehicleUseCase"/> class.
    /// </remarks>
    /// <param name="vehicleRepository">The vehicle repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="outputPort">The output port.</param>
    /// <param name="logger">The application logger.</param>
    public sealed class ReturnVehicleUseCase(
        IVehicleRepository vehicleRepository,
        IUnitOfWork unitOfWork,
        IReturnVehicleOutputPort outputPort,
        IAppLogger<ReturnVehicleUseCase> logger) : IUseCase<ReturnVehicleInput>
    {
        /// <summary>
        /// Executes the return vehicle use case.
        /// </summary>
        /// <param name="input">The input message.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Execute(ReturnVehicleInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            logger.LogInformation("Returning vehicle {VehicleId}.", input.VehicleId);

            var vehicle = await vehicleRepository.GetByIdAsync(new VehicleId(input.VehicleId));
            if (vehicle is null)
            {
                logger.LogWarning("Vehicle {VehicleId} not found.", input.VehicleId);
                outputPort.NotFoundHandle("Vehicle not found.");
                return;
            }

            vehicle.Return();

            await vehicleRepository.UpdateAsync(vehicle);
            await unitOfWork.Save();

            logger.LogInformation("Vehicle {VehicleId} returned successfully.", input.VehicleId);

            outputPort.StandardHandle(new ReturnVehicleOutput(vehicle.Id.Value));
        }
    }
}
