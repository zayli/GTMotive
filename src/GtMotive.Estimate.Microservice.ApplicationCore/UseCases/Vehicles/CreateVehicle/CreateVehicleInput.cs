namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.CreateVehicle
{
    /// <summary>
    /// Input message for the create vehicle use case.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CreateVehicleInput"/> class.
    /// </remarks>
    /// <param name="brand">The brand of the vehicle.</param>
    /// <param name="model">The model of the vehicle.</param>
    /// <param name="licensePlate">The license plate of the vehicle.</param>
    /// <param name="manufacturingDate">The manufacturing date of the vehicle.</param>
    public sealed class CreateVehicleInput(string brand, string model, string licensePlate, System.DateTime manufacturingDate) : IUseCaseInput
    {
        /// <summary>Gets the brand of the vehicle.</summary>
        public string Brand { get; } = brand;

        /// <summary>Gets the model of the vehicle.</summary>
        public string Model { get; } = model;

        /// <summary>Gets the license plate of the vehicle.</summary>
        public string LicensePlate { get; } = licensePlate;

        /// <summary>Gets the manufacturing date of the vehicle.</summary>
        public System.DateTime ManufacturingDate { get; } = manufacturingDate;
    }
}
