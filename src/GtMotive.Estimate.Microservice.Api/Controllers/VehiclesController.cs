using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.UseCases.Vehicles.CreateVehicle;
using GtMotive.Estimate.Microservice.Api.UseCases.Vehicles.GetAvailableVehicles;
using GtMotive.Estimate.Microservice.Api.UseCases.Vehicles.RentVehicle;
using GtMotive.Estimate.Microservice.Api.UseCases.Vehicles.ReturnVehicle;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    [ApiController]
    [Route("api/vehicles")]
    [AllowAnonymous]
    public sealed class VehiclesController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVehicleRequest request)
        {
            var presenter = await mediator.Send(request);
            return presenter.ActionResult;
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable()
        {
            var presenter = await mediator.Send(new GetAvailableVehiclesRequest());
            return presenter.ActionResult;
        }

        [HttpPost("{vehicleId:guid}/rent")]
        public async Task<IActionResult> Rent(Guid vehicleId, [FromBody] RentVehicleRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);
            request.VehicleId = vehicleId;
            var presenter = await mediator.Send(request);
            return presenter.ActionResult;
        }

        [HttpPost("{vehicleId:guid}/return")]
        public async Task<IActionResult> Return(Guid vehicleId)
        {
            var presenter = await mediator.Send(new ReturnVehicleRequest { VehicleId = vehicleId });
            return presenter.ActionResult;
        }
    }
}
