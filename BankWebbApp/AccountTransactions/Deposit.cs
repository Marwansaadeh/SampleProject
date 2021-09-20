using BankAppData.Models;
using System;

namespace BankWebbApp.AccountTransactions
{
    public class Deposit : Transaction, ITransctionGenerator
    {
        private readonly BankAppDataContext _context;
        public Deposit(BankAppDataContext context)
        {
            _context = context;
        }
        public Deposit()
        {

        }
        public void SaveTransaction()
        {
            if (ValidateOperation())
            {
                var Transaction = new Transactions();

                Transaction.AccountId = Account.AccountId;
                Transaction.Date = DateTime.Now;
                Transaction.Balance = Account.Balance + Amount; ;
                Transaction.Amount = Amount;
                Transaction.Type = "Credit";
                Transaction.Operation = "Credit in Cash";
                Transaction.Symbol = Symbol;

                _context.Transactions.Add(Transaction);
                Account.Balance = Account.Balance + Amount;

                _context.Accounts.Attach(Account);

                _context.SaveChanges();
            }

        }

        public bool ValidateOperation()
        {

            if (Amount >= 0 && Account != null)
            {
                return true;
            }
            else return false;
        }


    }
}
