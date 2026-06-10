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
            // Arrange: even with a valid route GUID, the body is missing the required
            // [JsonRequired] CustomerId, so model validation must short-circuit with 400.
            using var client = fixture.Server.CreateClient();
            var vehicleId = Guid.NewGuid();

            // Act
            using var response = await client.PostAsJsonAsync(
                $"/api/vehicles/{vehicleId}/rent",
                new { });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
