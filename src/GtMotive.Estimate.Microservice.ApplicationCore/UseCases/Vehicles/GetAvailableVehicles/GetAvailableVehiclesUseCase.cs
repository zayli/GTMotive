using System;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.GetAvailableVehicles
{
    /// <summary>Returns the vehicles currently available in the fleet.</summary>
    /// <param name="vehicleRepository">Vehicle repository.</param>
    /// <param name="outputPort">Output port.</param>
    /// <param name="logger">Application logger.</param>
    /// <param name="timeProvider">Clock used by the fleet age rule.</param>
    public sealed class GetAvailableVehiclesUseCase(
        IVehicleRepository vehicleRepository,
        IGetAvailableVehiclesOutputPort outputPort,
        IAppLogger<GetAvailableVehiclesUseCase> logger,
        TimeProvider timeProvider) : IUseCase<GetAvailableVehiclesInput>
    {
        /// <summary>Runs the use case.</summary>
        /// <param name="input">Input message.</param>
        /// <returns>Task.</returns>
        public async Task Execute(GetAvailableVehiclesInput input)
        {
            logger.LogInformation("Retrieving available vehicles.");

            var vehicles = await vehicleRepository.GetAvailableAsync();
            var utcNow = timeProvider.GetUtcNow().UtcDateTime;

            var items = vehicles
                .Where(vehicle => !vehicle.ExceedsFleetAgeLimit(utcNow))
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
