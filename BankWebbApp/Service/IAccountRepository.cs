using BankAppData.Models;
using System.Collections.Generic;

namespace BankWebbApp.Service
{
    public interface IAccountRepository
    {
        IEnumerable<Accounts> GetAccountsByCustomerID(int id);

        Accounts GetAccountByID(int id);


    }
}
