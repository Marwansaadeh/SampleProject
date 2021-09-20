//using BankAppData.Models;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using System;
//using System.Net.Http.Headers;
//using System.Security.Claims;
//using System.Text;
//using System.Text.Encodings.Web;
//using System.Threading.Tasks;

//namespace Mobilapp.Handlers
//{
//    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
//    {
//        private readonly BankAppDataContext _context;
//        private readonly SignInManager<IdentityUser> _signInManager;
//        public BasicAuthenticationHandler(
//    IOptionsMonitor<AuthenticationSchemeOptions> options,
//    ILoggerFactory logger,
//    UrlEncoder encoder,
//    ISystemClock clock,
//    BankAppDataContext context, SignInManager<IdentityUser> signInManager)
//    : base(options, logger, encoder, clock)
//        {
//            _context = context;
//            _signInManager = signInManager;
//        }
//        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
//        {
//            if (!Request.Headers.ContainsKey("Authorization"))
//            {
//                return AuthenticateResult.Fail("Authorization header was not found");

//            }
//            try
//            {
//                var authenticationHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
//                var bytes = Convert.FromBase64String(authenticationHeaderValue.Parameter);
//                string[] credentials = Encoding.UTF8.GetString(bytes).Split(":");
//                string email = credentials[0];
//                string pass = credentials[1];
//                var result = await _signInManager.PasswordSignInAsync(email,
//                               pass, false, lockoutOnFailure: true);
//                if (!result.Succeeded)
//                {
//                    return AuthenticateResult.Fail("Invalid username or password");

//                }
//                else
//                {
//                    var claims = new[] { new Claim(ClaimTypes.Name, email) };
//                    var identity = new ClaimsIdentity(claims, Scheme.Name);
//                    var princeple = new ClaimsPrincipal(identity);
//                    var ticket = new AuthenticationTicket(princeple, Scheme.Name);
//                    return AuthenticateResult.Success(ticket);
//                }
//            }
//            catch (Exception)
//            {

//                return AuthenticateResult.Fail("error has occured");

//            }

//        }
//    }
//}
