using MobilApp.Models;

namespace MobilApp.Services
{
    public interface IAuthentication
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
    }
}
