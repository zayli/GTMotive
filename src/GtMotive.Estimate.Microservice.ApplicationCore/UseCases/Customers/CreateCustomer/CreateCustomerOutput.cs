using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Customers.CreateCustomer
{
    /// <summary>
    /// Output message for the create customer use case.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CreateCustomerOutput"/> class.
    /// </remarks>
    /// <param name="id">The unique identifier of the created customer.</param>
    /// <param name="name">The full name of the customer.</param>
    /// <param name="documentId">The document identifier (DNI/NIE) of the customer.</param>
    public sealed class CreateCustomerOutput(Guid id, string name, string documentId) : IUseCaseOutput
    {
        /// <summary>Gets the unique identifier of the created customer.</summary>
        public Guid Id { get; } = id;

        /// <summary>Gets the full name of the customer.</summary>
        public string Name { get; } = name;

        /// <summary>Gets the document identifier (DNI/NIE) of the customer.</summary>
        public string DocumentId { get; } = documentId;
    }
}
