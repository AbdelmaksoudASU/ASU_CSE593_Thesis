using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace be.Models
{
    public class TokenDataRetrieval
    {
        public static string GetProfileIDFromToken(StringValues authHeader, TokenValidationParameters _tokenValidationParameters)
        {

            var token = authHeader.ToString().Split(" ")[1];
            var handler = new JwtSecurityTokenHandler();

            var claimsPrincipal = handler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = jwtToken.Claims.First(c => c.Type == "nameid").Value;

            return userId;
        }
        public static string GetProfileRoleFromToken(StringValues authHeader, TokenValidationParameters _tokenValidationParameters)
        {

            var token = authHeader.ToString().Split(" ")[1];
            var handler = new JwtSecurityTokenHandler();

            var claimsPrincipal = handler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;
            var userType = jwtToken.Claims.First(c => c.Type == "typ").Value;

            return userType;
        }
    }
}
