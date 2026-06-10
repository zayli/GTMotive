using AutoMapper;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.RentVehicle;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Vehicles.RentVehicle
{
    public sealed class RentVehiclePresenter : IWebApiPresenter, IRentVehicleOutputPort
    {
        private readonly IMapper mapper;

        public RentVehiclePresenter(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public IActionResult ActionResult { get; private set; }

        public void StandardHandle(RentVehicleOutput response)
        {
            var viewModel = mapper.Map<RentVehicleResponse>(response);
            ActionResult = new OkObjectResult(viewModel);
        }

        public void NotFoundHandle(string message)
        {
            ActionResult = new NotFoundObjectResult(message);
        }
    }
}
