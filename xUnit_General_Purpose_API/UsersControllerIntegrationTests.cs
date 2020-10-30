using Microsoft.AspNetCore.Mvc.Testing;
using MindMap_General_Purpose_API;
using System.Net;
using System.Net.Http;
using Xunit;

namespace xUnit_General_Purpose_API
{
    public class UsersControllerIntegrationTests
    {
        private readonly HttpClient _client;

        public UsersControllerIntegrationTests()
        {
            var appFactory = new WebApplicationFactory<Startup>();
            _client = appFactory.CreateClient(); // Set dev mongo env
        }

        [Fact]
        public async void ActivateUser_ShouldReturnOk() 
        {
            var response = await _client.GetAsync("https://localhost:6001/api/usermanagement/activate/5f95c5d26f4799e209733061");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
