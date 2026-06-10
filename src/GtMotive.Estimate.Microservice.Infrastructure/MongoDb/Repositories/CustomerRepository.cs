#nullable enable
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Documents;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Repositories
{
    public sealed class CustomerRepository : ICustomerRepository
    {
        private const string CollectionName = "customers";

        private readonly IMongoCollection<CustomerDocument> collection;

        public CustomerRepository(MongoService mongoService)
        {
            ArgumentNullException.ThrowIfNull(mongoService);
            collection = mongoService.GetCollection<CustomerDocument>(CollectionName);
        }

        public async Task AddAsync(Customer customer)
        {
            ArgumentNullException.ThrowIfNull(customer);
            await collection.InsertOneAsync(ToDocument(customer)).ConfigureAwait(false);
        }

        public async Task<Customer?> GetByIdAsync(CustomerId id)
        {
            var key = id.Value.ToString();
            var document = await collection
                .Find(x => x.Id == key)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            return document is null ? null : ToDomain(document);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            var documents = await collection
                .Find(FilterDefinition<CustomerDocument>.Empty)
                .ToListAsync()
                .ConfigureAwait(false);

            return documents.ConvertAll(ToDomain);
        }

        private static CustomerDocument ToDocument(Customer customer) => new()
        {
            Id = customer.Id.Value.ToString(),
            Name = customer.Name,
            DocumentId = customer.DocumentId
        };

        private static Customer ToDomain(CustomerDocument document) =>
            Customer.Rehydrate(new CustomerId(Guid.Parse(document.Id)), document.Name, document.DocumentId);
    }
}
