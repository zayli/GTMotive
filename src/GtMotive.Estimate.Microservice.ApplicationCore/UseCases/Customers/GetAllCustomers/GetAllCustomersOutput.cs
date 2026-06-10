using System.Collections.Generic;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Customers.GetAllCustomers
{
    /// <summary>
    /// Output message for the get all customers use case.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="GetAllCustomersOutput"/> class.
    /// </remarks>
    /// <param name="customers">The collection of customers.</param>
    public sealed class GetAllCustomersOutput(IEnumerable<CustomerItem> customers) : IUseCaseOutput
    {
        /// <summary>Gets the collection of customers.</summary>
        public IEnumerable<CustomerItem> Customers { get; } = customers;
    }
}
