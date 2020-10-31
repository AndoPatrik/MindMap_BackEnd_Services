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
        private readonly WebApplicationFactory<Startup> _appFactory;

        public UsersControllerIntegrationTests()
        {
            _appFactory = new WebApplicationFactory<Startup>();
            _client = _appFactory.CreateClient(); // Set dev mongo env
        }

        //TODO: Initialize few user records for testing
        private static void PrepareDataForActivateUserTests() { }

        //TODO: Clean Users collection in the test DB
        private static void CleanCollection() { }

        [Fact] //Change to theory
        public async void ActivateUser_ShouldReturnOk() 
        {
            var response = await _client.GetAsync("https://localhost:7001/api/usermanagement/activate/5f95c5d26f4799e209733061");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Your account has been updated. You can log in now." , await response.Content.ReadAsStringAsync());
        }

        [Theory]
        [InlineData("5f95c5d26f4799e209733hhh")]
        [InlineData("5f95c5d26f4799e2097ggggg")]
        [InlineData("5f95c5d26f4799e2097hjkle")]
        public async void ActivateUser_ShouldReturnNotFound(string userId) 
        {
            var response = await _client.GetAsync("https://localhost:7001/api/usermanagement/activate/" + userId);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("User does not exists." , await response.Content.ReadAsStringAsync());

        }
    }
}
