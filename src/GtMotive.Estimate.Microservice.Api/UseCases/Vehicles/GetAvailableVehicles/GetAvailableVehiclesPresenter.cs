using System;
using System.Collections.Generic;
using AutoMapper;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.GetAvailableVehicles;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Vehicles.GetAvailableVehicles
{
    public sealed class GetAvailableVehiclesPresenter : IWebApiPresenter, IGetAvailableVehiclesOutputPort
    {
        private readonly IMapper mapper;

        public GetAvailableVehiclesPresenter(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public IActionResult ActionResult { get; private set; }

        public void StandardHandle(GetAvailableVehiclesOutput response)
        {
            ArgumentNullException.ThrowIfNull(response);

            var items = mapper.Map<IEnumerable<AvailableVehicleResponse>>(response.Vehicles);
            ActionResult = new OkObjectResult(items);
        }
    }
}
