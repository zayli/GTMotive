using System;
using System.ComponentModel.DataAnnotations;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Vehicles.RentVehicle
{
    public sealed class RentVehicleResponse
    {
        [Required]
        public Guid VehicleId { get; set; }

        [Required]
        public Guid CustomerId { get; set; }
    }
}
