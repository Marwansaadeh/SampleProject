using BankAppData.Models;

namespace BankWebbApp.AccountTransactions
{
    public abstract class Transaction
    {
        public int TransactionId { get; set; }
        public Accounts Account { get; set; }
        public decimal Amount { get; set; }

        public string Symbol { get; set; }

    }
}
