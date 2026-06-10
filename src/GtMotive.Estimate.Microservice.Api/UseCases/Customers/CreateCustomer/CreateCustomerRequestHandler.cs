using System;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Customers.CreateCustomer;
using MediatR;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Customers.CreateCustomer
{
    public sealed class CreateCustomerRequestHandler(
        IUseCase<CreateCustomerInput> useCase,
        CreateCustomerPresenter presenter) : IRequestHandler<CreateCustomerRequest, IWebApiPresenter>
    {
        private readonly IUseCase<CreateCustomerInput> useCase = useCase;
        private readonly CreateCustomerPresenter presenter = presenter;

        public async Task<IWebApiPresenter> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var input = new CreateCustomerInput(request.Name, request.DocumentId);
            await useCase.Execute(input);
            return presenter;
        }
    }
}
