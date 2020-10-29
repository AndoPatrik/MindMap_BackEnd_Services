using MindMap_General_Purpose_API.Utils;
using System.Threading;
using Xunit;

namespace xUnit_General_Purpose_API
{
    public class HttpServiceTests
    {
        static object obj = new { name = "test", salary = 123, age = 50, id = 99 };

        [Fact(Skip = ("This test might be broken due to the API's policy (429 after multiple requests)"))]
        public async void PostAsync_ShouldReturnTrue() 
        {
            //ARRANGE
            //ACT
            bool IsTrue = await HttpService.PostAsync("http://dummy.restapiexample.com/api/v1/create", obj, CancellationToken.None);
            //ASSERT
            Assert.True(IsTrue);
        }
        
    }
}
