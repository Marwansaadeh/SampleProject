using BankAppData.Models;

namespace MobilApp.Models
{
    public class AuthenticateResponse
    {
        public int CustomerId { get; set; }
        public string Token { get; set; }
        public AuthenticateResponse(Customers user, string token)
        {
            CustomerId = user.CustomerId;
            Token = token;
        }

    }
}
