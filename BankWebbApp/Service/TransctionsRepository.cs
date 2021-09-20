using BankAppData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BankWebbApp.Service
{
    public class TransctionsRepository : ITransctionsRepository
    {
        private readonly BankAppDataContext _context;
        public TransctionsRepository(BankAppDataContext context)
        {
            _context = context;
        }
        public IEnumerable<Transactions> GetAccountsTransctions(int id)
        {
            return _context.Transactions.Where(x => x.AccountId == id);
        }

        public IEnumerable<Transactions> GetAccountTransctionsByOrder(int id)
        {
            return _context.Transactions.Where(x => x.AccountId == id).OrderByDescending(x => x.Date).ThenByDescending(x => x.TransactionId);
        }
        public IEnumerable<Transactions> GetAccountTransctionsBySpecificParameter(int customerid, int accountid, int tranasctionLimit, int offset)
        {

            if (tranasctionLimit == 0)
            {
                tranasctionLimit = 20;
            }
            if (tranasctionLimit > 0 && offset >= 0)
            {
                var accounttransactions = _context.Customers.Where(x => x.CustomerId == customerid).Include(x => x.Dispositions).SelectMany(x => x.Dispositions).Include(x => x.Account)
                      .Select(x => x.Account).SelectMany(x => x.Transactions).Where(x => x.AccountId == accountid);

                return accounttransactions.OrderByDescending(x => x.Date).Skip(offset).Take(tranasctionLimit).ToList();
            }
            return null;

        }

        public IEnumerable<Transactions> GetLastTreeDaysTransactions(DateTime startdate, DateTime finishdate)
        {
            return _context.Transactions.Where(x => x.Date >= startdate && x.Date <= finishdate);
        }
    }
}
