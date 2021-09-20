using BankWebbApp.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobilApp.Helpers;
using System.Linq;

namespace MobilApp.Controllers
{

    [Route("api/accounts")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        private readonly ITransctionsRepository _transctionsRepository;

        public AccountApiController(ITransctionsRepository transctionsRepository)
        {
            _transctionsRepository = transctionsRepository;
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Get(int id, int transctionLimit, int offset)
        {
            var customerId = TokenData.GetTokenId(HttpContext);
            var AccountTransctions = _transctionsRepository.GetAccountTransctionsBySpecificParameter(customerId, id, transctionLimit, offset);
            if (AccountTransctions == null)
            {
                return NotFound(new { message = "wrong parameter" });
            }
            else if (TokenData.GetTokenId(HttpContext) != 0 && AccountTransctions.Count() == 0)
            {
                return Unauthorized(new { message = "you don't have access to this account" });
            }
            else if (AccountTransctions.Any())
            {

                return Ok(AccountTransctions);
            }


            else
            {
                return NotFound();

            }

        }


    }



}

