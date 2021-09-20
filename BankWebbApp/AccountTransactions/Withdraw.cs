using BankAppData.Models;
using System;

namespace BankWebbApp.AccountTransactions
{
    public class Withdraw : Transaction, ITransctionGenerator
    {
        private readonly BankAppDataContext _context;
        public Withdraw(BankAppDataContext context)
        {
            _context = context;
        }
        public Withdraw()
        {

        }
        public void SaveTransaction()
        {
            if (ValidateOperation())
            {
                var Transaction = new Transactions();

                Transaction.AccountId = Account.AccountId;
                Transaction.Date = DateTime.Now;
                Transaction.Balance = Account.Balance - Amount;
                Transaction.Amount = -Amount;
                Transaction.Type = "Debit";
                Transaction.Operation = "Withdrawal in Cash";
                Transaction.Symbol = Symbol;

                _context.Transactions.Add(Transaction);
                Account.Balance = Account.Balance - Amount;

                _context.Accounts.Attach(Account);

                _context.SaveChanges();

            }

        }

        public bool ValidateOperation()
        {

            if (Amount <= Account.Balance && Account != null && Amount > 0)
            {
                return true;
            }
            else return false;
        }
    }
}
