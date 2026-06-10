using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Customers.GetAllCustomers
{
    /// <summary>
    /// Use case that retrieves every customer registered in the system.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="GetAllCustomersUseCase"/> class.
    /// </remarks>
    /// <param name="customerRepository">The customer repository.</param>
    /// <param name="outputPort">The output port.</param>
    /// <param name="logger">The application logger.</param>
    public sealed class GetAllCustomersUseCase(
        ICustomerRepository customerRepository,
        IGetAllCustomersOutputPort outputPort,
        IAppLogger<GetAllCustomersUseCase> logger) : IUseCase<GetAllCustomersInput>
    {
        /// <summary>
        /// Executes the get all customers use case.
        /// </summary>
        /// <param name="input">The input message.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Execute(GetAllCustomersInput input)
        {
            logger.LogInformation("Retrieving all customers.");

            var customers = await customerRepository.GetAllAsync();

            var items = customers
                .Select(customer => new CustomerItem(customer.Id.Value, customer.Name, customer.DocumentId))
                .ToList();

            logger.LogInformation("Returned {Count} customers.", items.Count);

            outputPort.StandardHandle(new GetAllCustomersOutput(items));
        }
    }
}
