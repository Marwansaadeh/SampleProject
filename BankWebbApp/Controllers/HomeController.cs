using BankAppData.Models;
using BankWebbApp.Models;
using BankWebbApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BankWebbApp.Controllers
{
    [Authorize(Roles = "Admin,Cashier")]
    public class HomeController : Controller
    {
        private readonly BankAppDataContext _context;
        public HomeController(BankAppDataContext context)
        {
            _context = context;
        }

        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]
        public IActionResult Index()
        {

            var model = new StatisticsViewModel();
            model.NumberOfAccounts = _context.Accounts.Count();
            model.NumberOfCustomers = _context.Customers.Count();
            model.SumofAccountsBalances = _context.Accounts.Sum(x => x.Balance);
            var countries = _context.Customers.Select(x => x.Country).Distinct().ToList();
            model.countryStatisticsViewModels = new List<CountryStatisticsViewModel>();
            foreach (var item in countries)
            {
                model.countryStatisticsViewModels.Add(
                    new CountryStatisticsViewModel
                    {
                        CountryName = item,
                        statisticsViewModel = new StatisticsViewModel
                        {
                            NumberOfAccounts = _context.Accounts.SelectMany(x => x.Dispositions).Where(x => x.Customer.Country == item).Select(x => x.Account).Distinct().Count(),

                            NumberOfCustomers = _context.Customers.Where(x => x.Country == item).Count(),

                            SumofAccountsBalances = _context.Accounts.SelectMany(x => x.Dispositions).Where(x => x.Customer.Country == item).Select(x => x.Account).Distinct().Sum(x => x.Balance)

                        }
                    }
                );

            };

            return View(model);
        }
        [ResponseCache(Duration = 120, VaryByQueryKeys = new[] { "CountryName" })]
        public IActionResult HighestAccounts(string CountryName)
        {

            var model = _context.Customers.Where(x => x.Country == CountryName).Include(j => j.Dispositions).SelectMany(x => x.Dispositions).Include(x => x.Account).Select(x => x.Account).OrderByDescending(x => x.Balance).Take(10).ToList();

            return View(model);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
