using System;

namespace GtMotive.Estimate.Microservice.Domain.Entities
{
    /// <summary>
    /// Represents a customer who can rent vehicles from the fleet.
    /// </summary>
    public sealed class Customer
    {
        private Customer()
        {
        }

        /// <summary>
        /// Gets the unique identifier of the customer.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the full name of the customer.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the document identifier (DNI/NIE) of the customer.
        /// </summary>
        public string DocumentId { get; private set; }

        /// <summary>
        /// Creates a new customer instance.
        /// </summary>
        /// <param name="name">The full name of the customer.</param>
        /// <param name="documentId">The document identifier (DNI/NIE) of the customer.</param>
        /// <returns>A new <see cref="Customer"/> instance.</returns>
        public static Customer Create(string name, string documentId)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new DomainException("Customer name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(documentId))
            {
                throw new DomainException("Customer document ID cannot be empty.");
            }

            return new Customer
            {
                Id = Guid.NewGuid(),
                Name = name,
                DocumentId = documentId.ToUpperInvariant().Trim()
            };
        }

        /// <summary>
        /// Reconstructs a customer from its persisted state.
        /// </summary>
        /// <param name="id">The unique identifier of the customer.</param>
        /// <param name="name">The full name of the customer.</param>
        /// <param name="documentId">The document identifier (DNI/NIE) of the customer.</param>
        /// <returns>A rehydrated <see cref="Customer"/> instance.</returns>
        public static Customer Rehydrate(Guid id, string name, string documentId)
        {
            return new Customer
            {
                Id = id,
                Name = name,
                DocumentId = documentId
            };
        }
    }
}
