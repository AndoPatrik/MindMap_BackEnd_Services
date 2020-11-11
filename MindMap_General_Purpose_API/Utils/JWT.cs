using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MindMap_General_Purpose_API.Utils
{
    public class JWT
    {
        public static string GenerateToken(string userId, string email)
        {
			var mySecret = "CRmvf{Mn2p84aCVmVWYAR]a6_qk)-;kD";
			var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));

			var myIssuer = "localhost";
			var myAudience = "localhost";

			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.NameIdentifier, userId),
					new Claim(ClaimTypes.Email, email )
				}),
				Expires = DateTime.UtcNow.AddHours(1),
				Issuer = myIssuer,
				Audience = myAudience,
				SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
    }
}
