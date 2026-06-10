using System;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.ReturnVehicle;
using MediatR;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Vehicles.ReturnVehicle
{
    public sealed class ReturnVehicleRequestHandler : IRequestHandler<ReturnVehicleRequest, IWebApiPresenter>
    {
        private readonly IUseCase<ReturnVehicleInput> useCase;
        private readonly ReturnVehiclePresenter presenter;

        public ReturnVehicleRequestHandler(
            IUseCase<ReturnVehicleInput> useCase,
            ReturnVehiclePresenter presenter)
        {
            this.useCase = useCase;
            this.presenter = presenter;
        }

        public async Task<IWebApiPresenter> Handle(ReturnVehicleRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var input = new ReturnVehicleInput(request.VehicleId);
            await useCase.Execute(input);
            return presenter;
        }
    }
}
