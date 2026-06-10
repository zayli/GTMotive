using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Repositories;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Settings;
using Microsoft.Extensions.Options;
using Xunit;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure
{
    // Functional tests assume MongoDB is reachable at localhost:27017. The repo's
    // docker-compose.yml exposes mongo there, so running `docker compose up` (or the
    // VS Docker Compose launch profile) is enough to make these tests pass.
    public sealed class MongoFixture : IAsyncLifetime
    {
        private const string ConnectionString = "mongodb://localhost:27017";

        // Per-test-run database so we never interfere with the dev database.
        private readonly string databaseName = $"gtmotive-estimate-tests-{Guid.NewGuid():N}";

        public MongoService MongoService { get; private set; }

        public IVehicleRepository VehicleRepository { get; private set; }

        public ICustomerRepository CustomerRepository { get; private set; }

        public IUnitOfWork UnitOfWork { get; private set; }

        public Task InitializeAsync()
        {
            var settings = Options.Create(new MongoDbSettings
            {
                ConnectionString = ConnectionString,
                MongoDbDatabaseName = databaseName,
            });

            MongoService = new MongoService(settings);
            VehicleRepository = new VehicleRepository(MongoService);
            CustomerRepository = new CustomerRepository(MongoService);
            UnitOfWork = new UnitOfWork();

            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            await MongoService.MongoClient.DropDatabaseAsync(databaseName).ConfigureAwait(false);
        }
    }
}
