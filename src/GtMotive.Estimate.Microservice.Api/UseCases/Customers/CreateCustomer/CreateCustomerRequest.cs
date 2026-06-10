using System.ComponentModel.DataAnnotations;
using MediatR;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Customers.CreateCustomer
{
    public sealed class CreateCustomerRequest : IRequest<IWebApiPresenter>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string DocumentId { get; set; }
    }
}
