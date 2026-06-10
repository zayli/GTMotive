using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.CreateVehicle
{
    /// <summary>
    /// Use case that creates a new vehicle and adds it to the fleet.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CreateVehicleUseCase"/> class.
    /// </remarks>
    /// <param name="vehicleRepository">The vehicle repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="outputPort">The standard output port.</param>
    /// <param name="logger">The application logger.</param>
    public sealed class CreateVehicleUseCase(
        IVehicleRepository vehicleRepository,
        IUnitOfWork unitOfWork,
        ICreateVehicleOutputPort outputPort,
        IAppLogger<CreateVehicleUseCase> logger) : IUseCase<CreateVehicleInput>
    {
        /// <summary>
        /// Executes the create vehicle use case.
        /// </summary>
        /// <param name="input">The input message.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
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

            var vehicle = Vehicle.Create(input.Brand, input.Model, licensePlate, manufacturingDate);

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
