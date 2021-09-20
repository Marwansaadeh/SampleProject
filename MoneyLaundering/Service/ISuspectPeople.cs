using BankAppData.Models;
using System.Collections.Generic;

namespace MoneyLaundering.Service
{
    public interface ISuspectPeople
    {
        SuspectPerson AddSuspectPerson(int AccountNumber, IEnumerable<Transactions> Transactions);
    }
}
