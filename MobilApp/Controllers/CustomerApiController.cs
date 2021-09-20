using BankWebbApp.Service;
using BankWebbApp.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobilApp.Helpers;
using MobilApp.Models;
using MobilApp.Services;
using System.Collections.Generic;
using System.Linq;

namespace MobilApp.Controllers
{
    [Route("api/me")]
    [ApiController]
    public class CustomerApiController : ControllerBase
    {
        private readonly IAuthentication _userAuthentication;
        private readonly ICustomerRepository _customerRepository;
        private readonly IAccountRepository _accountRepositoryRepository;


        public CustomerApiController(IAuthentication userAuthentication, ICustomerRepository customerRepository, IAccountRepository accountRepository)
        {
            _customerRepository = customerRepository;
            _accountRepositoryRepository = accountRepository;
            _userAuthentication = userAuthentication;

        }
        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userAuthentication.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "CustomerId is incorrect" });

            return Ok(response);
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        [HttpGet("get")]

        public IActionResult Get()
        {
            var customer = _customerRepository.GetCustomerByID(TokenData.GetTokenId(HttpContext));
            if (customer != null)
            {
                var model = new CustomerDetailsViewModel();
                var accounts = _accountRepositoryRepository.GetAccountsByCustomerID(customer.CustomerId);
                model.CustomerId = customer.CustomerId;
                model.Birthday = customer.Birthday;
                model.City = customer.City;
                model.Country = customer.Country;
                model.CountryCode = customer.CountryCode;
                model.Emailaddress = customer.Emailaddress;
                model.Gender = customer.Gender;
                model.Givenname = customer.Givenname;
                model.Surname = customer.Surname;
                model.Telephonecountrycode = customer.Telephonecountrycode;
                model.Telephonenumber = customer.Telephonenumber;
                model.Zipcode = customer.Zipcode;
                model.Totalbalance = accounts.Sum(x => x.Balance);

                var list = new List<CustomerAccountsSummaryViewModel>();
                foreach (var item in accounts)
                {
                    var modelcustomeraccountsummary = new CustomerAccountsSummaryViewModel();
                    modelcustomeraccountsummary.Accounts = item.AccountId;
                    modelcustomeraccountsummary.AccountsBalances = item.Balance;
                    list.Add(modelcustomeraccountsummary);

                }
                model.CustomerAccountsSummary = list;
                return Ok(model);
            }
            else
            {
                return NotFound();
            }


        }
    }
}
