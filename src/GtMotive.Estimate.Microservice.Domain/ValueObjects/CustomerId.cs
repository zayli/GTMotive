using System;

namespace GtMotive.Estimate.Microservice.Domain.ValueObjects
{
    /// <summary>
    /// Strongly-typed identifier for a customer.
    /// </summary>
    /// <param name="Value">The underlying GUID value.</param>
    public readonly record struct CustomerId(Guid Value);
}
