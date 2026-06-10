using AutoMapper;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Customers.CreateCustomer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Customers.CreateCustomer
{
    public sealed class CreateCustomerPresenter : IWebApiPresenter, ICreateCustomerOutputPort
    {
        private readonly IMapper mapper;

        public CreateCustomerPresenter(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public IActionResult ActionResult { get; private set; }

        public void StandardHandle(CreateCustomerOutput response)
        {
            var viewModel = mapper.Map<CreateCustomerResponse>(response);
            ActionResult = new ObjectResult(viewModel) { StatusCode = StatusCodes.Status201Created };
        }
    }
}
