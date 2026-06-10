using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.GetAvailableVehicles;
using MediatR;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Vehicles.GetAvailableVehicles
{
    public sealed class GetAvailableVehiclesRequestHandler : IRequestHandler<GetAvailableVehiclesRequest, IWebApiPresenter>
    {
        private readonly IUseCase<GetAvailableVehiclesInput> useCase;
        private readonly GetAvailableVehiclesPresenter presenter;

        public GetAvailableVehiclesRequestHandler(
            IUseCase<GetAvailableVehiclesInput> useCase,
            GetAvailableVehiclesPresenter presenter)
        {
            this.useCase = useCase;
            this.presenter = presenter;
        }

        public async Task<IWebApiPresenter> Handle(GetAvailableVehiclesRequest request, CancellationToken cancellationToken)
        {
            await useCase.Execute(new GetAvailableVehiclesInput());
            return presenter;
        }
    }
}
