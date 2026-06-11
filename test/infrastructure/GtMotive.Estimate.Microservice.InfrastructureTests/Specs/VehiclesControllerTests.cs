using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure;
using Xunit;

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Specs
{
    [Collection(TestCollections.TestServer)]
    public class VehiclesControllerTests(GenericInfrastructureTestServerFixture fixture)
    {
        private readonly GenericInfrastructureTestServerFixture fixture = fixture;

        [Fact]
        public async Task Rent_WhenBodyIsMissingCustomerId_ShouldReturnBadRequest()
        {
            using var client = fixture.Server.CreateClient();
            var vehicleId = Guid.NewGuid();

            using var response = await client.PostAsJsonAsync(
                $"/api/vehicles/{vehicleId}/rent",
                new { });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
