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
            using var client = fixture.Server.CreateClient();

            using var response = await client.PostAsJsonAsync("/api/customers", new { });

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
