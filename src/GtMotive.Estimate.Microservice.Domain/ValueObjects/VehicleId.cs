using System;

namespace GtMotive.Estimate.Microservice.Domain.ValueObjects
{
    /// <summary>
    /// Strongly-typed identifier for a vehicle.
    /// </summary>
    /// <param name="Value">The underlying GUID value.</param>
    public readonly record struct VehicleId(Guid Value);
}
