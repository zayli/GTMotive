using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Documents;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb
{
    public class MongoService
    {
        private readonly IMongoDatabase database;

        public MongoService(IOptions<MongoDbSettings> options)
        {
            MongoClient = new MongoClient(options.Value.ConnectionString);
            database = MongoClient.GetDatabase(options.Value.MongoDbDatabaseName);

            EnsureIndexes();
        }

        public MongoClient MongoClient { get; }

        public IMongoCollection<TDocument> GetCollection<TDocument>(string name)
        {
            return database.GetCollection<TDocument>(name);
        }

        private void EnsureIndexes()
        {
            var vehicles = database.GetCollection<VehicleDocument>("vehicles");
            var plateIndex = new CreateIndexModel<VehicleDocument>(
                Builders<VehicleDocument>.IndexKeys.Ascending(x => x.LicensePlate),
                new CreateIndexOptions { Unique = true, Name = "uniq_license_plate" });
            vehicles.Indexes.CreateOne(plateIndex);
        }
    }
}
