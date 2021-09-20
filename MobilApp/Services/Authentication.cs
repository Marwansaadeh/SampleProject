using BankAppData.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MobilApp.Helpers;
using MobilApp.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace MobilApp.Services
{
    public class Authentication : IAuthentication
    {
        private readonly AppSettings _appSettings;
        private readonly BankAppDataContext _context;
        public Authentication(IOptions<AppSettings> appSettings, BankAppDataContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {

            var user = _context.Customers.SingleOrDefault(x => x.CustomerId == model.CustomerId);
            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user.CustomerId);

            return new AuthenticateResponse(user, token);
        }


        private string generateJwtToken(int user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("CustomerId", user.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
