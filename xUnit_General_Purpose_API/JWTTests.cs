using Microsoft.IdentityModel.Tokens;
using MindMap_General_Purpose_API.Utils;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;
using System.Text;
using Xunit;

namespace xUnit_General_Purpose_API
{
    public class JWTTests
    {
        private static bool ValidateToken(string authToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters()
            {
                ValidateLifetime = false,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidIssuer = "localhost",
                ValidAudience = "localhost",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("CRmvf{Mn2p84aCVmVWYAR]a6_qk)-;kD"))
            };

            SecurityToken validatedToken;
            IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
            return true;
        }

        [Theory]
        [InlineData("randomuserid1")]
        [InlineData("randomuserid2")]
        [InlineData("randomuserid3")]
        public void GenerateJWT_GeneratedTokenShouldBeValid(string userId) 
        {
            //ARRANGE
            //ACT
            string token = JWT.GenerateToken(userId);
            //ASSERT
            Assert.True(ValidateToken(token));
        }
    }
}
