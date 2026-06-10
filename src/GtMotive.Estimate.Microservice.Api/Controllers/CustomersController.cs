using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.UseCases.Customers.CreateCustomer;
using GtMotive.Estimate.Microservice.Api.UseCases.Customers.GetAllCustomers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    [ApiController]
    [Route("api/customers")]
    [AllowAnonymous]
    public sealed class CustomersController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request)
        {
            var presenter = await mediator.Send(request);
            return presenter.ActionResult;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var presenter = await mediator.Send(new GetAllCustomersRequest());
            return presenter.ActionResult;
        }
    }
}
