using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MediatR;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Vehicles.RentVehicle
{
    public sealed class RentVehicleRequest : IRequest<IWebApiPresenter>
    {
        // Set by the controller from the route, not posted by the client.
        [JsonIgnore]
        public Guid VehicleId { get; set; }

        [Required]
        [JsonRequired]
        public Guid CustomerId { get; set; }
    }
}
