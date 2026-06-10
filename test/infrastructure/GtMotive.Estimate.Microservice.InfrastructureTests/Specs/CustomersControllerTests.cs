using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure;
using Xunit;

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Specs
{
    [Collection(TestCollections.TestServer)]
    public class CustomersControllerTests(GenericInfrastructureTestServerFixture fixture)
    {
        private readonly GenericInfrastructureTestServerFixture fixture = fixture;

        [Fact]
        public async Task Create_WhenBodyIsEmpty_ShouldReturnBadRequest()
        {
            // Arrange: model validation must reject missing required Name and DocumentId
            // before the request reaches the use case (so no Mongo is needed for this test).
            using var client = fixture.Server.CreateClient();

            // Act
            using var response = await client.PostAsJsonAsync("/api/customers", new { });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
