using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Customers.GetAllCustomers;
using MediatR;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Customers.GetAllCustomers
{
    public sealed class GetAllCustomersRequestHandler(
        IUseCase<GetAllCustomersInput> useCase,
        GetAllCustomersPresenter presenter) : IRequestHandler<GetAllCustomersRequest, IWebApiPresenter>
    {
        public async Task<IWebApiPresenter> Handle(GetAllCustomersRequest request, CancellationToken cancellationToken)
        {
            await useCase.Execute(new GetAllCustomersInput());
            return presenter;
        }
    }
}
