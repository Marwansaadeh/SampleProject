using BankAppData.Models;
using BankWebbApp.AccountTransactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankApp.Tests
{
    [TestClass]
    public class AccountTests
    {
        [TestMethod]
        public void WhenCustomerTransferMoreMoneyThanHisBalanceAccount_ReturnFalse()
        {
            var Account = new Accounts();
            Account.AccountId = 1;
            Account.Balance = 200;

            var RecieverAccount = new Accounts();
            RecieverAccount.AccountId = 5;

            var transfer = new Transfer();
            transfer.TransactionId = 1;

            transfer.Account = Account;
            transfer.Account.Balance = Account.Balance;
            transfer.ReceivedAccount = RecieverAccount;
            transfer.Amount = 500;
            Assert.IsFalse(transfer.ValidateOperation());
        }

        [TestMethod]
        public void WhenCustomerWithdarwMoreMoneyThanHisBalanceAccount_ReturnFalse()
        {
            var Account = new Accounts();
            Account.AccountId = 1;
            Account.Balance = 200;

            var Withdraw = new Withdraw();
            Withdraw.TransactionId = 1;
            Withdraw.Amount = 300;

            Withdraw.Account = Account;

            Assert.IsFalse(Withdraw.ValidateOperation());
        }

        [TestMethod]
        public void WhenCustomerTypesNegativeAmountWhenWithdrawMoney_ReturnFalse()
        {
            var Account = new Accounts();
            Account.AccountId = 1;
            Account.Balance = 200;

            var Withdraw = new Withdraw();
            Withdraw.TransactionId = 1;
            Withdraw.Amount = -300;

            Withdraw.Account = Account;

            Assert.IsFalse(Withdraw.ValidateOperation());
        }

        [TestMethod]
        public void WhenCustomerTypesNegativeAmountWhenTransferMoneyToAnotherAccount_ReturnFalse()
        {
            var Account = new Accounts();
            Account.AccountId = 1;
            Account.Balance = 200;

            var RecieverAccount = new Accounts();
            RecieverAccount.AccountId = 5;

            var transfer = new Transfer();
            transfer.TransactionId = 1;

            transfer.Account = Account;
            transfer.Account.Balance = Account.Balance;
            transfer.ReceivedAccount = RecieverAccount;
            transfer.Amount = -500;
            Assert.IsFalse(transfer.ValidateOperation());

        }

    }
}
