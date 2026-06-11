using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.CreateVehicle
{
    /// <summary>Adds a new vehicle to the fleet.</summary>
    /// <param name="vehicleRepository">Vehicle repository.</param>
    /// <param name="unitOfWork">Unit of work.</param>
    /// <param name="outputPort">Output port.</param>
    /// <param name="logger">Application logger.</param>
    /// <param name="timeProvider">Clock used by the fleet age rule.</param>
    public sealed class CreateVehicleUseCase(
        IVehicleRepository vehicleRepository,
        IUnitOfWork unitOfWork,
        ICreateVehicleOutputPort outputPort,
        IAppLogger<CreateVehicleUseCase> logger,
        TimeProvider timeProvider) : IUseCase<CreateVehicleInput>
    {
        /// <summary>Runs the use case.</summary>
        /// <param name="input">Input message.</param>
        /// <returns>Task.</returns>
        public async Task Execute(CreateVehicleInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            logger.LogInformation(
                "Creating vehicle {Brand} {Model} with license plate {LicensePlate}.",
                input.Brand,
                input.Model,
                input.LicensePlate);

            var licensePlate = new LicensePlate(input.LicensePlate);
            var manufacturingDate = new ManufacturingDate(input.ManufacturingDate);

            if (await vehicleRepository.ExistsByLicensePlateAsync(licensePlate))
            {
                logger.LogWarning(
                    "A vehicle with license plate {LicensePlate} already exists.",
                    licensePlate.Value);
                throw new DomainException("A vehicle with the same license plate already exists.");
            }

            var vehicle = Vehicle.Create(input.Brand, input.Model, licensePlate, manufacturingDate, timeProvider.GetUtcNow().UtcDateTime);

            await vehicleRepository.AddAsync(vehicle);
            await unitOfWork.Save();

            logger.LogInformation("Vehicle {VehicleId} created successfully.", vehicle.Id.Value);

            outputPort.StandardHandle(new CreateVehicleOutput(
                vehicle.Id.Value,
                vehicle.Brand,
                vehicle.Model,
                vehicle.LicensePlate.Value));
        }
    }
}
