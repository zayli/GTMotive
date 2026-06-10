using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MediatR;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Vehicles.CreateVehicle
{
    public sealed class CreateVehicleRequest : IRequest<IWebApiPresenter>
    {
        [Required]
        public string Brand { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public string LicensePlate { get; set; }

        [Required]
        [JsonRequired]
        public DateTime ManufacturingDate { get; set; }
    }
}
