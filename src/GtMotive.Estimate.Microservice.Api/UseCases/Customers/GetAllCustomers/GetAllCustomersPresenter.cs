using System;
using System.Collections.Generic;
using AutoMapper;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Customers.GetAllCustomers;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Customers.GetAllCustomers
{
    public sealed class GetAllCustomersPresenter(IMapper mapper) : IWebApiPresenter, IGetAllCustomersOutputPort
    {
        public IActionResult ActionResult { get; private set; }

        public void StandardHandle(GetAllCustomersOutput response)
        {
            ArgumentNullException.ThrowIfNull(response);

            var items = mapper.Map<IEnumerable<CustomerResponse>>(response.Customers);
            ActionResult = new OkObjectResult(items);
        }
    }
}
