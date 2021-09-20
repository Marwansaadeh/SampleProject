using BankAppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankWebbApp.Service
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly BankAppDataContext _context;

        public CustomerRepository(BankAppDataContext _context)
        {
            this._context = _context;
        }

        public IEnumerable<Customers> GetCustomers()
        {
            return _context.Customers;
        }
        public Customers GetCustomerByID(int customerId)
        {
            return _context.Customers.FirstOrDefault(x => x.CustomerId == customerId);
        }

        public void InsertCustomer(Customers customer)
        {

            Dispositions dispositions = new Dispositions();
            Accounts accounts = new Accounts();

            accounts.Created = DateTime.Now;
            accounts.Frequency = "Monthly";
            accounts.Balance = 0.0m;

            _context.Accounts.Add(accounts);

            _context.Customers.Add(customer);
            _context.SaveChanges();

            dispositions.AccountId = accounts.AccountId;
            dispositions.CustomerId = customer.CustomerId;
            dispositions.Type = "OWNER";
            _context.Dispositions.Add(dispositions);
            _context.SaveChanges();

        }

        public void DeleteCustomer(int customerId)
        {
            Customers customer = _context.Customers.Find(customerId);
            _context.Customers.Remove(customer);
        }

        public void UpdateCustomer(Customers customer)
        {
            _context.Customers.Attach(customer);
            _context.SaveChanges();

        }

        public Customers FindCustomerByAccountNumber(int accountnumber)
        {
            return _context.Dispositions.Where(x => x.AccountId == accountnumber).Select(x => x.Customer).First();
        }
    }
}
