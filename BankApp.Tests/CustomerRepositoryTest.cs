using BankAppData.Models;
using BankWebbApp.Controllers;
using BankWebbApp.Service;
using BankWebbApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankApp.Tests
{
    [TestClass]
    public class CustomerRepositoryTest
    {
        private CustomerController sut;


        [TestMethod]
        public void CustomerRepo_ReturnCorrectData()
        {
            var option = new DbContextOptionsBuilder<BankAppDataContext>().UseInMemoryDatabase(databaseName: "RepoReturnData").EnableSensitiveDataLogging().Options;
            var context = new BankAppDataContext(option);
            Seed(context);
            var customerrep = new CustomerRepository(context);
            var accountrepo = new AccountRepository(context);

            sut = new CustomerController(customerrep, accountrepo);
            var result = sut.CustomerDetails(1) as ViewResult;
            var model = result.Model as CustomerDetailsViewModel;

            Assert.AreEqual(1, model.CustomerId);
            Assert.AreEqual(2, model.CustomerAccountsSummary.Count());

        }



        [TestMethod]
        public void AccountRepo_ReturnCustomerAccount()
        {
            var option = new DbContextOptionsBuilder<BankAppDataContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var context = new BankAppDataContext(option);
            Seed(context);

            var query = new AccountRepository(context);
            var result = query.GetAccountsByCustomerID(1).ToList();

            Assert.AreEqual(2, result.Count);

        }
        private void Seed(BankAppDataContext bankAppDataContext)
        {
            var customer = new List<Customers>() {

               new Customers{CustomerId =1}

               };
            var dispositions = new List<Dispositions>() {

               new Dispositions{ AccountId=1, CustomerId=1, DispositionId=1},
               new Dispositions{ AccountId=2, CustomerId=1, DispositionId=2}

               };
            var accounts = new List<Accounts>() {

               new Accounts{AccountId =1},
               new Accounts{AccountId=2}

               };
            bankAppDataContext.Accounts.AddRange(accounts);

            bankAppDataContext.Customers.AddRange(customer);
            bankAppDataContext.Dispositions.AddRange(dispositions);

            bankAppDataContext.SaveChanges();
        }

    }
}
