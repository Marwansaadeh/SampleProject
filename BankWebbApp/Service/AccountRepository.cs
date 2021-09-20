using BankAppData.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BankWebbApp.Service
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BankAppDataContext _context;
        public AccountRepository(BankAppDataContext context)
        {
            _context = context;
        }
        public IEnumerable<Accounts> GetAccountsByCustomerID(int id)
        {
            return _context.Customers.Include(x => x.Dispositions).Where(x => x.CustomerId == id).SelectMany(x => x.Dispositions).Include(x => x.Account)
                 .Select(x => x.Account);
        }

        public Accounts GetAccountByID(int id)
        {
            return _context.Accounts.FirstOrDefault(x => x.AccountId == id);
        }


    }
}
