#nullable enable
using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.Interfaces
{
    /// <summary>
    /// Defines the repository contract for customer persistence operations.
    /// </summary>
    public interface ICustomerRepository
    {
        /// <summary>
        /// Adds a new customer to the repository.
        /// </summary>
        /// <param name="customer">The customer to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddAsync(Customer customer);

        /// <summary>
        /// Retrieves a customer by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the customer.</param>
        /// <returns>The customer if found; otherwise null.</returns>
        Task<Customer?> GetByIdAsync(CustomerId id);

        /// <summary>
        /// Retrieves all registered customers.
        /// </summary>
        /// <returns>A collection with every customer in the repository.</returns>
        Task<IEnumerable<Customer>> GetAllAsync();
    }
}
