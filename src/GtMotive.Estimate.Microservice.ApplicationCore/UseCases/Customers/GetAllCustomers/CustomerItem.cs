using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Customers.GetAllCustomers
{
    /// <summary>
    /// Customer projection returned by the get all customers use case.
    /// </summary>
    /// <param name="Id">The unique identifier of the customer.</param>
    /// <param name="Name">The full name of the customer.</param>
    /// <param name="DocumentId">The document identifier (DNI/NIE) of the customer.</param>
    public sealed record CustomerItem(Guid Id, string Name, string DocumentId);
}
