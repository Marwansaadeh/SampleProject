using BankAppData.Models;
using BankWebbApp.AccountTransactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BankApp.Tests
{
    [TestClass]

    public class WithdrawTransctionTest
    {
        [TestMethod]
        public void WithdrawClass_SaveCorrectTransctionAndUpateAccountBlanceAfterTransction()
        {


            var mockSet = new Mock<DbSet<Transactions>>();
            var mockContext = new Mock<BankAppDataContext>();
            mockContext.Setup(m => m.Transactions).Returns(mockSet.Object);

            var mockSet2 = new Mock<DbSet<Accounts>>();
            mockContext.Setup(m => m.Accounts).Returns(mockSet2.Object);

            var withdraw1 = new Withdraw(mockContext.Object);

            var Account = new Accounts();
            Account.AccountId = 50070;
            Account.Balance = 700;



            withdraw1.Account = Account;
            withdraw1.Amount = 400;

            withdraw1.SaveTransaction();

            mockSet.Verify(m => m.Add(It.IsAny<Transactions>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
            mockSet.Verify(m => m.Add(It.Is<Transactions>(r => r.Amount == -400m && r.AccountId == 50070 && r.Balance == 300 && r.Type == "Debit")));
            Assert.IsTrue(Account.Balance == 300);

        }
    }
}

