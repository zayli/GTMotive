namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Customers.CreateCustomer
{
    /// <summary>
    /// Input message for the create customer use case.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CreateCustomerInput"/> class.
    /// </remarks>
    /// <param name="name">The full name of the customer.</param>
    /// <param name="documentId">The document identifier (DNI/NIE) of the customer.</param>
    public sealed class CreateCustomerInput(string name, string documentId) : IUseCaseInput
    {
        /// <summary>Gets the full name of the customer.</summary>
        public string Name { get; } = name;

        /// <summary>Gets the document identifier (DNI/NIE) of the customer.</summary>
        public string DocumentId { get; } = documentId;
    }
}
