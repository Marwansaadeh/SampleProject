using BankAppData.Models;
using System;

namespace BankWebbApp.AccountTransactions
{
    public class Transfer : Transaction, ITransctionGenerator
    {
        public string Bank { get; set; }
        public Accounts ReceivedAccount { get; set; }

        private readonly BankAppDataContext _context;
        public Transfer(BankAppDataContext context)
        {
            _context = context;
        }
        public Transfer()
        {

        }
        public void SaveTransaction()
        {
            if (ValidateOperation())
            {

                var Transaction2 = new Transactions();

                Transaction2.AccountId = Account.AccountId;
                Transaction2.Date = DateTime.Now;
                Transaction2.Balance = Account.Balance - Amount;
                Transaction2.Amount = -Amount;
                Transaction2.Type = "Debit";
                Transaction2.Operation = "Withdrawal in Cash";
                Transaction2.Symbol = Symbol;
                Account.Balance = Account.Balance - Amount;

                var Transaction = new Transactions();
                Transaction.AccountId = ReceivedAccount.AccountId;
                Transaction.Date = DateTime.Now;
                Transaction.Balance = ReceivedAccount.Balance + Amount;
                Transaction.Amount = Amount;
                Transaction.Type = "Credit";
                Transaction.Operation = "Collection from Another Bank";
                Transaction.Symbol = Symbol;
                Transaction.Bank = Bank;
                Transaction.AccountId = ReceivedAccount.AccountId;

                ReceivedAccount.Balance = ReceivedAccount.Balance + Amount;
                Transaction.Account = Account.AccountId.ToString();



                _context.Accounts.Attach(Account);
                _context.Accounts.Attach(ReceivedAccount);

                _context.Transactions.Add(Transaction);
                _context.Transactions.Add(Transaction2);

                _context.SaveChanges();
            }

        }
        public bool ValidateOperation()
        {

            if (Amount > 0 && Account != null && ReceivedAccount != null && Amount <= Account.Balance && ReceivedAccount.AccountId != Account.AccountId)
            {
                return true;
            }
            else return false;
        }
    }
}
