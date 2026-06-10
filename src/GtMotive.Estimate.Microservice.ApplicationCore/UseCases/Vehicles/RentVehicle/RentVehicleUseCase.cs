using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.RentVehicle
{
    /// <summary>
    /// Use case that rents an available vehicle to a customer.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="RentVehicleUseCase"/> class.
    /// </remarks>
    /// <param name="vehicleRepository">The vehicle repository.</param>
    /// <param name="customerRepository">The customer repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="outputPort">The output port.</param>
    /// <param name="logger">The application logger.</param>
    public sealed class RentVehicleUseCase(
        IVehicleRepository vehicleRepository,
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork,
        IRentVehicleOutputPort outputPort,
        IAppLogger<RentVehicleUseCase> logger) : IUseCase<RentVehicleInput>
    {
        /// <summary>
        /// Executes the rent vehicle use case.
        /// </summary>
        /// <param name="input">The input message.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Execute(RentVehicleInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            logger.LogInformation(
                "Renting vehicle {VehicleId} for customer {CustomerId}.",
                input.VehicleId,
                input.CustomerId);

            var vehicleId = new VehicleId(input.VehicleId);
            var customerId = new CustomerId(input.CustomerId);

            var vehicle = await vehicleRepository.GetByIdAsync(vehicleId);
            if (vehicle is null)
            {
                logger.LogWarning("Vehicle {VehicleId} not found.", input.VehicleId);
                outputPort.NotFoundHandle("Vehicle not found.");
                return;
            }

            var customer = await customerRepository.GetByIdAsync(customerId);
            if (customer is null)
            {
                logger.LogWarning("Customer {CustomerId} not found.", input.CustomerId);
                outputPort.NotFoundHandle("Customer not found.");
                return;
            }

            if (await vehicleRepository.HasActiveRentalByCustomerAsync(customerId))
            {
                logger.LogWarning(
                    "Customer {CustomerId} already has an active rental.",
                    input.CustomerId);
                throw new DomainException("Customer already has an active rental.");
            }

            vehicle.Rent(customerId);

            await vehicleRepository.UpdateAsync(vehicle);
            await unitOfWork.Save();

            logger.LogInformation(
                "Vehicle {VehicleId} successfully rented to customer {CustomerId}.",
                input.VehicleId,
                input.CustomerId);

            outputPort.StandardHandle(new RentVehicleOutput(vehicle.Id.Value, customerId.Value));
        }
    }
}
