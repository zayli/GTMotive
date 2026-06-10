using System;
using System.ComponentModel.DataAnnotations;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Customers.CreateCustomer
{
    public sealed class CreateCustomerResponse
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string DocumentId { get; set; }
    }
}
