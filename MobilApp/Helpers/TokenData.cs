using Microsoft.AspNetCore.Http;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace MobilApp.Helpers
{
    public static class TokenData
    {
        public static int GetTokenId(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            var tokenValues = new JwtSecurityTokenHandler().ReadJwtToken(token);

            var customerId = tokenValues.Claims.FirstOrDefault(x => x.Type == "CustomerId").Value;
            return Int32.Parse(customerId);

        }
    }
}
