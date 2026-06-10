using System;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.RentVehicle;
using MediatR;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Vehicles.RentVehicle
{
    public sealed class RentVehicleRequestHandler : IRequestHandler<RentVehicleRequest, IWebApiPresenter>
    {
        private readonly IUseCase<RentVehicleInput> useCase;
        private readonly RentVehiclePresenter presenter;

        public RentVehicleRequestHandler(
            IUseCase<RentVehicleInput> useCase,
            RentVehiclePresenter presenter)
        {
            this.useCase = useCase;
            this.presenter = presenter;
        }

        public async Task<IWebApiPresenter> Handle(RentVehicleRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var input = new RentVehicleInput(request.VehicleId, request.CustomerId);
            await useCase.Execute(input);
            return presenter;
        }
    }
}
