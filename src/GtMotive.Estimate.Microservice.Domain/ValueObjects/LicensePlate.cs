namespace GtMotive.Estimate.Microservice.Domain.ValueObjects
{
    /// <summary>
    /// Represents a vehicle's license plate.
    /// </summary>
    public sealed class LicensePlate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LicensePlate"/> class.
        /// </summary>
        /// <param name="value">The license plate string value.</param>
        public LicensePlate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new DomainException("License plate cannot be empty.");
            }

            Value = value.ToUpperInvariant().Trim();
        }

        /// <summary>
        /// Gets the license plate value.
        /// </summary>
        public string Value { get; }
    }
}
