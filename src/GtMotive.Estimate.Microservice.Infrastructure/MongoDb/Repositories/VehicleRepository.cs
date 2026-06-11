#nullable enable
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Enums;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Documents;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Repositories
{
    public sealed class VehicleRepository : IVehicleRepository
    {
        private const string CollectionName = "vehicles";

        private readonly IMongoCollection<VehicleDocument> collection;

        public VehicleRepository(MongoService mongoService)
        {
            ArgumentNullException.ThrowIfNull(mongoService);
            collection = mongoService.GetCollection<VehicleDocument>(CollectionName);
        }

        public async Task AddAsync(Vehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle);
            await collection.InsertOneAsync(ToDocument(vehicle)).ConfigureAwait(false);
        }

        public async Task<Vehicle?> GetByIdAsync(VehicleId id)
        {
            var key = id.Value.ToString();
            var document = await collection
                .Find(x => x.Id == key)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            return document is null ? null : ToDomain(document);
        }

        public async Task<IEnumerable<Vehicle>> GetAvailableAsync()
        {
            var availableStatus = VehicleStatus.Available.ToString();
            var documents = await collection
                .Find(x => x.Status == availableStatus)
                .ToListAsync()
                .ConfigureAwait(false);

            return documents.ConvertAll(ToDomain);
        }

        public async Task UpdateAsync(Vehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle);
            var key = vehicle.Id.Value.ToString();
            await collection
                .ReplaceOneAsync(x => x.Id == key, ToDocument(vehicle))
                .ConfigureAwait(false);
        }

        public async Task<bool> HasActiveRentaAsync(CustomerId customerId)
        {
            var key = customerId.Value.ToString();
            var rentedStatus = VehicleStatus.Rented.ToString();
            var count = await collection
                .CountDocumentsAsync(x => x.RentedByCustomerId == key && x.Status == rentedStatus)
                .ConfigureAwait(false);

            return count > 0;
        }

        public async Task<bool> ExistsByLicensePlateAsync(LicensePlate licensePlate)
        {
            ArgumentNullException.ThrowIfNull(licensePlate);
            var plate = licensePlate.Value;
            var count = await collection
                .CountDocumentsAsync(x => x.LicensePlate == plate)
                .ConfigureAwait(false);

            return count > 0;
        }

        private static VehicleDocument ToDocument(Vehicle vehicle) => new()
        {
            Id = vehicle.Id.Value.ToString(),
            Brand = vehicle.Brand,
            Model = vehicle.Model,
            LicensePlate = vehicle.LicensePlate.Value,
            ManufacturingDate = vehicle.ManufacturingDate.Value,
            Status = vehicle.Status.ToString(),
            RentedByCustomerId = vehicle.RentedByCustomerId?.Value.ToString()
        };

        private static Vehicle ToDomain(VehicleDocument document)
        {
            CustomerId? rentedBy = string.IsNullOrEmpty(document.RentedByCustomerId)
                ? null
                : new CustomerId(Guid.Parse(document.RentedByCustomerId));

            return Vehicle.Rehydrate(
                new VehicleId(Guid.Parse(document.Id)),
                document.Brand,
                document.Model,
                new LicensePlate(document.LicensePlate),
                new ManufacturingDate(document.ManufacturingDate),
                Enum.Parse<VehicleStatus>(document.Status),
                rentedBy);
        }
    }
}
