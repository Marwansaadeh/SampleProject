using BankAppData.Models;
using System;
using System.Collections.Generic;

namespace BankWebbApp.Service
{
    public interface ITransctionsRepository
    {
        IEnumerable<Transactions> GetAccountsTransctions(int id);
        IEnumerable<Transactions> GetAccountTransctionsByOrder(int id);
        IEnumerable<Transactions> GetAccountTransctionsBySpecificParameter(int customerid, int accountid, int tranasctionLimit, int offset);
        IEnumerable<Transactions> GetLastTreeDaysTransactions(DateTime startdate, DateTime finishdate);
    }
}
