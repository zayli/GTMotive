using System;
using System.Text.Json.Serialization;
using MediatR;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Vehicles.ReturnVehicle
{
    public sealed class ReturnVehicleRequest : IRequest<IWebApiPresenter>
    {
        // Set by the controller from the route, not posted by the client.
        [JsonIgnore]
        public Guid VehicleId { get; set; }
    }
}
