using System;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Customers.CreateCustomer;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.ApplicationCore
{
    public class CreateCustomerUseCaseTests
    {
        [Fact]
        public async Task Execute_WithValidInput_ShouldPersistAndCallStandardOutput()
        {
            var customerRepo = new Mock<ICustomerRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var outputPort = new Mock<ICreateCustomerOutputPort>();

            var useCase = new CreateCustomerUseCase(
                customerRepo.Object,
                unitOfWork.Object,
                outputPort.Object,
                Mock.Of<IAppLogger<CreateCustomerUseCase>>());

            await useCase.Execute(new CreateCustomerInput("Alice", "12345678A"));

            customerRepo.Verify(r => r.AddAsync(It.IsAny<Customer>()), Times.Once);
            unitOfWork.Verify(u => u.Save(), Times.Once);
            outputPort.Verify(o => o.StandardHandle(It.Is<CreateCustomerOutput>(x => x.Name == "Alice")), Times.Once);
        }

        [Fact]
        public async Task Execute_WithBlankName_ShouldThrowDomainException()
        {
            var useCase = new CreateCustomerUseCase(
                Mock.Of<ICustomerRepository>(),
                Mock.Of<IUnitOfWork>(),
                Mock.Of<ICreateCustomerOutputPort>(),
                Mock.Of<IAppLogger<CreateCustomerUseCase>>());

            Func<Task> act = () => useCase.Execute(new CreateCustomerInput("   ", "12345678A"));

            await act.Should().ThrowAsync<DomainException>();
        }
    }
}
