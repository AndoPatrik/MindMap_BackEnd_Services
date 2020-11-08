using MindMap_General_Purpose_API;
using MindMap_General_Purpose_API.Models;
using Newtonsoft.Json;
using SharedResources.SharedTests;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace xUnit_General_Purpose_API
{
    public class UsersControllerIntegrationTests : IClassFixture<WebApplicationFactoryWithTestMongo<Startup>>
    {
        private readonly HttpClient _client;

        public UsersControllerIntegrationTests(WebApplicationFactoryWithTestMongo<Startup> appFactory)
        {
            _client = appFactory.CreateClient();
            // TODO: method to make 5 users in MongoDB + clear DB after use
            // clear function is last one in file+runs every time we run tests!
            // READ UP ON: in memory mongodb (MongoToGo)
        }

        //TODO: Initialize few user records for testing
        private static void PrepareDataForActivateUserTests() { }

        //TODO: Clean Users collection in the test DB
        private static void CleanCollection() { }

        //--------------------------------ActivateUser-----------------------------------------------
        [Fact] //Change to theory
        public async void ActivateUser_ShouldReturnOk() 
        {
            var response = await _client.GetAsync("api/usermanagement/activate/5fa2ce6efe435d28620dc18e");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Your account has been updated. You can log in now." , await response.Content.ReadAsStringAsync());
        }

        [Theory]
        [InlineData("5f95c5d26f4799e209733hhh")]
        [InlineData("5f95c5d26f4799e2097ggggg")]
        [InlineData("5f95c5d26f4799e2097hjkle")]
        public async void ActivateUser_ShouldReturnNotFound(string userId) 
        {
            var response = await _client.GetAsync("api/usermanagement/activate/" + userId);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("User does not exists." , await response.Content.ReadAsStringAsync());
        }

        //--------------------------------AuthenticateUser-----------------------------------------------
        [Theory]
        [InlineData("test@gmail.com", "secret")]
        //[InlineData("test1@gmail.com", "secret1")]
        public async void AuthenticateUser_AuthenticationSuccess_ShouldReturnOk(string email, string password)
        {
            User input = new User(email: email, password: password);
            var data = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/usermanagement/authenticate/", data);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("test1@gmail.com", "secret1")]
        public async void AuthenticateUser_UserDoesNotExists_ShouldReturnBadRequest(string email, string password) 
        {
            User input = new User(email: email, password: password);
            var data = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/usermanagement/authenticate/", data);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("test@gmail.com", "secret")]
        public async void AuthenticateUser_AccountNotActivated_ShouldReturnBadRequest(string email, string password)
        {
            User input = new User(email: email, password: password);
            var data = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/usermanagement/authenticate/", data);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        //--------------------------------RegistrUser---------------------------------------------------
        [Theory]
        [InlineData("test@gmail.com","secret")]
        [InlineData("test1@gmail.com","secret1")]
        [InlineData("test2@gmail.com","secret2")]
        public async void RegisterUser_SuccessfulRegistarton_ShouldReturnOk(string email, string password) // Brakes due to the email api
        {
            User input = new User(email: email, password: password);
            var data = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/usermanagement/registration", data);
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
