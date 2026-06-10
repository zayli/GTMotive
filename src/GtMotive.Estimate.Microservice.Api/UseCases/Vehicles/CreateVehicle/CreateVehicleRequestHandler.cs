using System;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.CreateVehicle;
using MediatR;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Vehicles.CreateVehicle
{
    public sealed class CreateVehicleRequestHandler : IRequestHandler<CreateVehicleRequest, IWebApiPresenter>
    {
        private readonly IUseCase<CreateVehicleInput> useCase;
        private readonly CreateVehiclePresenter presenter;

        public CreateVehicleRequestHandler(
            IUseCase<CreateVehicleInput> useCase,
            CreateVehiclePresenter presenter)
        {
            this.useCase = useCase;
            this.presenter = presenter;
        }

        public async Task<IWebApiPresenter> Handle(CreateVehicleRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var input = new CreateVehicleInput(
                request.Brand,
                request.Model,
                request.LicensePlate,
                request.ManufacturingDate);

            await useCase.Execute(input);

            return presenter;
        }
    }
}
