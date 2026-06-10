using AutoMapper;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.CreateVehicle;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Vehicles.CreateVehicle
{
    public sealed class CreateVehiclePresenter : IWebApiPresenter, ICreateVehicleOutputPort
    {
        private readonly IMapper mapper;

        public CreateVehiclePresenter(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public IActionResult ActionResult { get; private set; }

        public void StandardHandle(CreateVehicleOutput response)
        {
            var viewModel = mapper.Map<CreateVehicleResponse>(response);
            ActionResult = new ObjectResult(viewModel) { StatusCode = StatusCodes.Status201Created };
        }
    }
}
