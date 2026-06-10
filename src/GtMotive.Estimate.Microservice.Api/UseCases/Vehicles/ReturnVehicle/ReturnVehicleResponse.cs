using System;
using System.ComponentModel.DataAnnotations;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Vehicles.ReturnVehicle
{
    public sealed class ReturnVehicleResponse
    {
        [Required]
        public Guid VehicleId { get; set; }
    }
}
