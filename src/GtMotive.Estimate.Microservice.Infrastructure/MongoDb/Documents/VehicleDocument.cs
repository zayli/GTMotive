using System;
using MongoDB.Bson.Serialization.Attributes;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Documents
{
    public sealed class VehicleDocument
    {
        [BsonId]
        public string Id { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public string LicensePlate { get; set; }

        public DateTime ManufacturingDate { get; set; }

        public string Status { get; set; }

        public string RentedByCustomerId { get; set; }
    }
}
