using BankAppData.Models;
using BankWebbApp.Controllers;
using BankWebbApp.Service;
using BankWebbApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BankApp.Tests
{
    [TestClass]
    public class CustomerTestController
    {
        private CustomerController sut;
        private Mock<ICustomerRepository> customerepo;
        private Mock<IAccountRepository> accountrepo;
        private Mock<ILogger<CustomerController>> logger;

        [TestInitialize]
        public void Intialize()
        {
            customerepo = new Mock<ICustomerRepository>();
            accountrepo = new Mock<IAccountRepository>();
            logger = new Mock<ILogger<CustomerController>>();

            sut = new CustomerController(customerepo.Object, accountrepo.Object);
        }
        [TestMethod]
        public void CustomerDetailsShouldReturnCorrectView()
        {
            customerepo.Setup(x => x.GetCustomerByID(1)).Returns(new Customers());

            var result = sut.CustomerDetails(1) as ViewResult;
            Assert.IsNull(result.ViewName);
        }
        [TestMethod]
        public void CustomerDetailsShouldReturnCorrectViewModel()
        {
            customerepo.Setup(x => x.GetCustomerByID(1)).Returns(new Customers());
            var result = sut.CustomerDetails(1) as ViewResult;
            Assert.IsInstanceOfType(result.Model, typeof(CustomerDetailsViewModel));
        }
        [TestMethod]
        public void CustomerDetailsShouldReturnCorrectDatabAboutApecificCustomer()
        {
            customerepo.Setup(x => x.GetCustomerByID(1)).Returns(new Customers()
            {
                CustomerId = 1,
                Birthday = new DateTime(1970, 07, 05),
                City = "JESSHEIM",
                Country = "mm",
                CountryCode = "NO",
                Emailaddress = "HannaStormo@rhyta.com",
                Gender = "female",
                Givenname = "Hanna",
                Surname = "Stormo",
                Telephonecountrycode = "47",
                Telephonenumber = "451 58 247",
                Zipcode = "2050"
            });


            var accounts = accountrepo.Setup(x => x.GetAccountsByCustomerID(1)).Returns(new List<Accounts>() {
           new Accounts {AccountId=1, Balance=90},
            new Accounts {AccountId=2, Balance=90},
             new Accounts {AccountId=3, Balance=90},

           });

            var result = sut.CustomerDetails(1) as ViewResult;
            var model = result.Model as CustomerDetailsViewModel;
            Assert.AreEqual(1, model.CustomerId);
            Assert.AreEqual("Hanna", model.Givenname);

            Assert.AreEqual(270, model.Totalbalance);
            Assert.AreEqual(3, model.CustomerAccountsSummary.Count());

        }

    }
}
