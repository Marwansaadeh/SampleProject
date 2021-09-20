using BankAppData.Models;
using System.Collections.Generic;

namespace BankWebbApp.Service
{
    public interface ICustomerRepository
    {
        IEnumerable<Customers> GetCustomers();
        Customers GetCustomerByID(int customerId);
        void InsertCustomer(Customers customer);
        void UpdateCustomer(Customers customer);

        Customers FindCustomerByAccountNumber(int accountnumber);


    }
}
