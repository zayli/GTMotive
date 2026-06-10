using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Customers.CreateCustomer
{
    /// <summary>
    /// Use case that registers a new customer.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CreateCustomerUseCase"/> class.
    /// </remarks>
    /// <param name="customerRepository">The customer repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="outputPort">The output port.</param>
    /// <param name="logger">The application logger.</param>
    public sealed class CreateCustomerUseCase(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork,
        ICreateCustomerOutputPort outputPort,
        IAppLogger<CreateCustomerUseCase> logger) : IUseCase<CreateCustomerInput>
    {
        /// <summary>
        /// Executes the create customer use case.
        /// </summary>
        /// <param name="input">The input message.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Execute(CreateCustomerInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            logger.LogInformation(
                "Creating customer {Name} with document {DocumentId}.",
                input.Name,
                input.DocumentId);

            var customer = Customer.Create(input.Name, input.DocumentId);

            await customerRepository.AddAsync(customer);
            await unitOfWork.Save();

            logger.LogInformation("Customer {CustomerId} created successfully.", customer.Id.Value);

            outputPort.StandardHandle(new CreateCustomerOutput(
                customer.Id.Value,
                customer.Name,
                customer.DocumentId));
        }
    }
}
