using BankAppData.Models;
using BankWebbApp.Service;
using BankWebbApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankWebbApp.Controllers
{
    [Authorize(Roles = "Admin,Cashier")]
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _CustomerRepository;
        private readonly IAccountRepository _AccountRepository;

        public CustomerController(ICustomerRepository customerRepository, IAccountRepository accountRepository)
        {

            _CustomerRepository = customerRepository;
            _AccountRepository = accountRepository;
        }

        [HttpGet]
        public IActionResult CustomerDetails(int CustomerID)
        {
            var model = new CustomerDetailsViewModel();

            var customer = _CustomerRepository.GetCustomerByID(CustomerID);

            if (customer != null)
            {
                var customersaccounts = _AccountRepository.GetAccountsByCustomerID(CustomerID);

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
                model.Totalbalance = customersaccounts.Sum(x => x.Balance);

                var lista = new List<CustomerAccountsSummaryViewModel>();
                foreach (var item in customersaccounts)
                {
                    var modelcustomeraccountsummary = new CustomerAccountsSummaryViewModel();
                    modelcustomeraccountsummary.Accounts = item.AccountId;
                    modelcustomeraccountsummary.AccountsBalances = item.Balance;
                    lista.Add(modelcustomeraccountsummary);

                }
                model.CustomerAccountsSummary = lista;

            }
            else
            {
                return View("ResultNotFound", CustomerID);

            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Customers(string search, string page, string columnname, string sort)
        {
            var model = new CustomersViewModel();
            var query = _CustomerRepository.GetCustomers().Select(c => new Items()
            {
                CustomerId = c.CustomerId,
                NationalId = c.NationalId,
                Givenname = c.Givenname,
                City = c.City,
                Streetaddress = c.Streetaddress
            });

            int currentPage = string.IsNullOrEmpty(page) ? 1 : Convert.ToInt32(page);
            model.Search = search;

            model.PagingViewModel.CurrentPage = currentPage;

            if (string.IsNullOrEmpty(search))
            {
                var pageCount = (double)query.Count() / PagingViewModel.PageSize;
                model.PagingViewModel.MaxPages = (int)Math.Ceiling(pageCount);

                query = AddSorting(query, ref columnname, ref sort);

                query = query.Skip((currentPage - 1) * PagingViewModel.PageSize).Take(PagingViewModel.PageSize);

                model.PagingViewModel.CurrentPage = currentPage;
                model.Items = query.ToList();
                model.PagingViewModel.Sort = sort;
                model.PagingViewModel.ColumnName = columnname;
                return View(model);
            }
            else
            {

                var searchResult = query.Where(s => s.City.StartsWith(search) || s.City == search || s.Givenname.StartsWith(search) || s.Givenname == search);
                searchResult = AddSorting(searchResult, ref columnname, ref sort); ;
                var pageCount = (double)searchResult.Count() / PagingViewModel.PageSize;
                model.PagingViewModel.MaxPages = (int)Math.Ceiling(pageCount);

                searchResult = searchResult.Skip((currentPage - 1) * PagingViewModel.PageSize).Take(PagingViewModel.PageSize);

                model.Items = searchResult.ToList();
                model.PagingViewModel.Sort = sort;
                model.PagingViewModel.ColumnName = columnname;
                return View(model);
            }

        }
        [HttpGet]
        public IActionResult Search(string search, string page, string columnname, string sort)
        {
            var model = new CustomersViewModel();
            var query = _CustomerRepository.GetCustomers().Select(c => new Items()
            {
                CustomerId = c.CustomerId,
                NationalId = c.NationalId,
                Givenname = c.Givenname,
                City = c.City,
                Streetaddress = c.Streetaddress
            });

            int currentPage = string.IsNullOrEmpty(page) ? 1 : Convert.ToInt32(page);
            model.Search = search;

            model.PagingViewModel.CurrentPage = currentPage;

            if (string.IsNullOrEmpty(search))
            {
                var pageCount = (double)query.Count() / PagingViewModel.PageSize;
                model.PagingViewModel.MaxPages = (int)Math.Ceiling(pageCount);

                query = AddSorting(query, ref columnname, ref sort);

                query = query.Skip((currentPage - 1) * PagingViewModel.PageSize).Take(PagingViewModel.PageSize);

                model.PagingViewModel.CurrentPage = currentPage;
                model.Items = query.ToList();
                model.PagingViewModel.Sort = sort;
                model.PagingViewModel.ColumnName = columnname;
                return PartialView("_CustomersPartial", model);
            }
            else
            {

                var searchResult = query.Where(s => s.City.StartsWith(search) || s.City == search || s.Givenname.StartsWith(search) || s.Givenname == search);
                searchResult = AddSorting(searchResult, ref columnname, ref sort); ;
                var pageCount = (double)searchResult.Count() / PagingViewModel.PageSize;
                model.PagingViewModel.MaxPages = (int)Math.Ceiling(pageCount);

                searchResult = searchResult.Skip((currentPage - 1) * PagingViewModel.PageSize).Take(PagingViewModel.PageSize);

                model.Items = searchResult.ToList();
                model.PagingViewModel.Sort = sort;
                model.PagingViewModel.ColumnName = columnname;
                model.PagingViewModel.CurrentPage = Convert.ToInt32(page);
                return PartialView("_CustomersPartial", model);
            }

        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateCustomer()
        {
            CustomerViewModel model = new CustomerViewModel();
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult CreatedCustomer(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                Customers customers = new Customers();
                customers.Birthday = model.Birthday;
                customers.City = model.City;
                customers.Country = model.Country;
                customers.CountryCode = model.CountryCode;
                customers.Emailaddress = model.Emailaddress;
                customers.Gender = model.Gender;
                customers.Givenname = model.Givenname;
                customers.Streetaddress = model.Streetaddress;
                customers.Surname = model.Surname;
                customers.Telephonecountrycode = model.Telephonecountrycode;
                customers.Telephonenumber = model.Telephonenumber;
                customers.Zipcode = model.Zipcode;
                customers.NationalId = model.NationalId;
                _CustomerRepository.InsertCustomer(customers);
                model.SucessMessage = "A new Customer has been added";
                return View("CreateCustomer", model);
            }
            return View(model);


        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult EditCustomer(int id)
        {
            CustomerViewModel model = new CustomerViewModel();
            var customers = _CustomerRepository.GetCustomerByID(id);
            model.Birthday = customers.Birthday;
            model.City = customers.City;
            model.Country = customers.Country;
            model.CountryCode = customers.CountryCode;
            model.Emailaddress = customers.Emailaddress;
            model.Gender = customers.Gender;
            model.Givenname = customers.Givenname;
            model.Streetaddress = customers.Streetaddress;
            model.Surname = customers.Surname;
            model.Telephonecountrycode = customers.Telephonecountrycode;
            model.Telephonenumber = customers.Telephonenumber;
            model.Zipcode = customers.Zipcode;
            model.NationalId = customers.NationalId;
            model.CustomerId = customers.CustomerId;
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult EditCustomer(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customers = _CustomerRepository.GetCustomerByID(model.CustomerId);
                customers.Birthday = model.Birthday;
                customers.City = model.City;
                customers.Country = model.Country;
                customers.CountryCode = model.CountryCode;
                customers.Emailaddress = model.Emailaddress;
                customers.Gender = model.Gender;
                customers.Givenname = model.Givenname;
                customers.Streetaddress = model.Streetaddress;
                customers.Surname = model.Surname;
                customers.Telephonecountrycode = model.Telephonecountrycode;
                customers.Telephonenumber = model.Telephonenumber;
                customers.Zipcode = model.Zipcode;
                customers.NationalId = model.NationalId;
                _CustomerRepository.UpdateCustomer(customers);
                model.SucessMessage = "Customer has been updated";
                return RedirectToAction("Customers");
            }
            return View(model);

        }
        private IEnumerable<Items> AddSorting(IEnumerable<Items> query, ref string sortcolumn, ref string sortorder)
        {

            if (string.IsNullOrEmpty(sortcolumn))
                sortcolumn = "CustomerId";
            if (string.IsNullOrEmpty(sortorder))
                sortorder = "asc";

            switch (sortcolumn)
            {
                case "CustomerId":
                    if (sortorder == "asc")
                    {
                        query = query.OrderBy(x => x.CustomerId);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.CustomerId);
                    }
                    break;
                case "GivenName":
                    if (sortorder == "asc")
                    {
                        query = query.OrderBy(x => x.Givenname);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Givenname);
                    }
                    break;
                case "Address":
                    if (sortorder == "asc")
                    {
                        query = query.OrderBy(x => x.Streetaddress);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Streetaddress);
                    }
                    break;
                case "City":
                    if (sortorder == "asc")
                    {
                        query = query.OrderBy(x => x.City);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.City);
                    }
                    break;
                case "NationalID":
                    if (sortorder == "asc")
                    {
                        query = query.OrderBy(x => x.NationalId);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.NationalId);
                    }
                    break;

            }
            return query;
        }

    }
}