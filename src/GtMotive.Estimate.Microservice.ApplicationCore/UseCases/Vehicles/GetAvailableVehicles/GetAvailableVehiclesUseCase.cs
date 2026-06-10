using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.GetAvailableVehicles
{
    /// <summary>
    /// Use case that retrieves all vehicles currently available in the fleet.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="GetAvailableVehiclesUseCase"/> class.
    /// </remarks>
    /// <param name="vehicleRepository">The vehicle repository.</param>
    /// <param name="outputPort">The standard output port.</param>
    /// <param name="logger">The application logger.</param>
    public sealed class GetAvailableVehiclesUseCase(
        IVehicleRepository vehicleRepository,
        IGetAvailableVehiclesOutputPort outputPort,
        IAppLogger<GetAvailableVehiclesUseCase> logger) : IUseCase<GetAvailableVehiclesInput>
    {
        /// <summary>
        /// Executes the get available vehicles use case.
        /// </summary>
        /// <param name="input">The input message.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Execute(GetAvailableVehiclesInput input)
        {
            logger.LogInformation("Retrieving available vehicles.");

            var vehicles = await vehicleRepository.GetAvailableAsync();

            var items = vehicles
                .Where(vehicle => !vehicle.ExceedsFleetAgeLimit())
                .Select(vehicle => new AvailableVehicleItem(
                    vehicle.Id.Value,
                    vehicle.Brand,
                    vehicle.Model,
                    vehicle.LicensePlate.Value))
                .ToList();

            logger.LogInformation("Returned {Count} available vehicles.", items.Count);

            outputPort.StandardHandle(new GetAvailableVehiclesOutput(items));
        }
    }
}
