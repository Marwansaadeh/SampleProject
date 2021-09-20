using BankAppData.Models;
using System.Collections.Generic;

namespace MoneyLaundering
{
    public class SuspectPerson
    {
        public SuspectPerson()
        {
            this.Transactions = new List<Transactions>();
        }
        public int Accountnumber { get; set; }
        public string CustomerName { get; set; }
        public decimal Amount { get; set; }
        public string Country { get; set; }

        public List<Transactions> Transactions { get; set; }
    }
}
