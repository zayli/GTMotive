using AutoMapper;
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

namespace GtMotive.Estimate.Microservice.Api.Mapping
{
    public sealed class ApiProfile : Profile
    {
        public ApiProfile()
        {
            CreateMap<CreateVehicleOutput, CreateVehicleResponse>();
            CreateMap<AvailableVehicleItem, AvailableVehicleResponse>();
            CreateMap<RentVehicleOutput, RentVehicleResponse>();
            CreateMap<ReturnVehicleOutput, ReturnVehicleResponse>();
            CreateMap<CreateCustomerOutput, CreateCustomerResponse>();
            CreateMap<CustomerItem, CustomerResponse>();
        }
    }
}
