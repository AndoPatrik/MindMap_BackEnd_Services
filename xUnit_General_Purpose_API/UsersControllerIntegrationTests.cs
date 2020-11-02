using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MindMap_General_Purpose_API;
using MindMap_General_Purpose_API.Models;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace xUnit_General_Purpose_API
{
    public class UsersControllerIntegrationTests
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Startup> _appFactory;

        public UsersControllerIntegrationTests()
        {
            _appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder => 
                {
                    builder.ConfigureServices(services => 
                    {
                        services.RemoveAll(typeof(MongoClient));
                        services.AddSingleton<MongoClient>(new MongoClient("mongodb+srv://admin:mindmap2020@mindmappercluster.gtnqi.mongodb.net/<dbname>?retryWrites=true&w=majority"));
                    });
                });
        
            _client = _appFactory.CreateClient(); // Set dev mongo env
        }

        //TODO: Initialize few user records for testing
        private static void PrepareDataForActivateUserTests() { }

        //TODO: Clean Users collection in the test DB
        private static void CleanCollection() { }

        //--------------------------------ActivateUser-----------------------------------------------
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

        //--------------------------------AuthenticateUser-----------------------------------------------
        [Fact]
        public void AuthenticateUser_AuthenticationSuccess_ShouldReturnOk()
        {

        }

        [Fact]
        public void AuthenticateUser_UserDoesNotExists_ShouldReturnBadRequest() 
        {

        }

        [Fact]
        public void AuthenticateUser_AccountNotActivated_ShouldReturnBadRequest()
        {

        }

        //--------------------------------RegistrUser---------------------------------------------------
        [Theory]
        [InlineData("test@gmail.com","secret")]
        [InlineData("test1@gmail.com","secret1")]
        [InlineData("test2@gmail.com","secret2")]
        public async void RegisterUser_SuccessfulRegistarton_ShouldReturnOk(string email, string password) 
        {
            User input = new User(email: email, password: password);
            var data = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("https://localhost:7001/api/usermanagement/registration", data);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void RegisterUser_InvalidUserData_ShouldReturnBadRequest()
        {

        }

        [Fact]
        public void RegisterUser_FailedRegistartion_ShouldReturnNotFound()
        {

        }
    }
}
