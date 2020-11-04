using MindMap_General_Purpose_API.Models;
using MindMap_General_Purpose_API.Utils;
using Xunit;

namespace xUnit_General_Purpose_API
{
    public class ValidateUtilTests
    {
        [Theory]
        [InlineData("test67@gmail.com", "test67")]
        [InlineData("test68@gmail.com", "test68")]
        [InlineData("test68@gmail.com", "test69")]
        public void ValidateUser_ShouldReturnValid(string email, string pw)
        {
            //ARRANGE
            User testUser = new User();
            testUser.Email = email;
            testUser.Password = pw;
            //ACT
            bool actual = ValidatorUtil.ValidateUser(testUser);
            //ASSERT
            Assert.True(actual);
        }
    }
}
