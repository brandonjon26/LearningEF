using LearningEF.Api;
using LearningEF.Api.Data;
using LearningEF.Api.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;


namespace LearningEF.IntegrationTests
{
    // IClassFixture ensures the factory is initialized once for the entire class
    public class CarControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;

        public CarControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetCars_ReturnsUnauthorized_WhenNoTokenIsProvided()
        {
            // ARRANGE: Create a client that will NOT send a token
            var client = _factory.CreateClient();

            // ACT
            var response = await client.GetAsync("/api/car");

            // ASSERT
            // Controller is protected by [Authorize], so we expect 401
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}