using AutoMapper;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.ReturnVehicle;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Vehicles.ReturnVehicle
{
    public sealed class ReturnVehiclePresenter : IWebApiPresenter, IReturnVehicleOutputPort
    {
        private readonly IMapper mapper;

        public ReturnVehiclePresenter(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public IActionResult ActionResult { get; private set; }

        public void StandardHandle(ReturnVehicleOutput response)
        {
            var viewModel = mapper.Map<ReturnVehicleResponse>(response);
            ActionResult = new OkObjectResult(viewModel);
        }

        public void NotFoundHandle(string message)
        {
            ActionResult = new NotFoundObjectResult(message);
        }
    }
}
