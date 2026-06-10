using System;
using GtMotive.Estimate.Microservice.Api.Mapping;
using GtMotive.Estimate.Microservice.Api.UseCases.Customers.CreateCustomer;
using GtMotive.Estimate.Microservice.Api.UseCases.Customers.GetAllCustomers;
using GtMotive.Estimate.Microservice.Api.UseCases.Vehicles.CreateVehicle;
using GtMotive.Estimate.Microservice.Api.UseCases.Vehicles.GetAvailableVehicles;
using GtMotive.Estimate.Microservice.Api.UseCases.Vehicles.RentVehicle;
using GtMotive.Estimate.Microservice.Api.UseCases.Vehicles.ReturnVehicle;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Customers.CreateCustomer;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Customers.GetAllCustomers;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.CreateVehicle;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.GetAvailableVehicles;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.RentVehicle;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.ReturnVehicle;
using Microsoft.Extensions.DependencyInjection;

namespace GtMotive.Estimate.Microservice.Api.DependencyInjection
{
    public static class UserInterfaceExtensions
    {
        public static IServiceCollection AddPresenters(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services
                .AddPresenter<CreateVehiclePresenter, ICreateVehicleOutputPort>()
                .AddPresenter<GetAvailableVehiclesPresenter, IGetAvailableVehiclesOutputPort>()
                .AddPresenter<RentVehiclePresenter, IRentVehicleOutputPort>()
                .AddPresenter<ReturnVehiclePresenter, IReturnVehicleOutputPort>()
                .AddPresenter<CreateCustomerPresenter, ICreateCustomerOutputPort>()
                .AddPresenter<GetAllCustomersPresenter, IGetAllCustomersOutputPort>();

            services.AddAutoMapper(cfg => cfg.AddProfile<ApiProfile>());

            return services;
        }

        // Registers a presenter as both its concrete type (so the request handler can inject
        // it) and as the output port the use case depends on. Scoped so both sides share the
        // same instance per HTTP request.
        private static IServiceCollection AddPresenter<TPresenter, TOutputPort>(this IServiceCollection services)
            where TPresenter : class, TOutputPort
            where TOutputPort : class
        {
            services.AddScoped<TPresenter>();
            services.AddScoped<TOutputPort>(sp => sp.GetRequiredService<TPresenter>());
            return services;
        }
    }
}
