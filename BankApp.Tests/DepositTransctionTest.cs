using BankAppData.Models;
using BankWebbApp.AccountTransactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BankApp.Tests
{
    [TestClass]
    public class DepositTransctionTest
    {
        [TestMethod]
        public void DepositClass_SaveCorrectTransctionAndUpateAccountBlanceAfterTransction()
        {
            var mockSet = new Mock<DbSet<Transactions>>();
            var mockContext = new Mock<BankAppDataContext>();
            mockContext.Setup(m => m.Transactions).Returns(mockSet.Object);

            var mockSet2 = new Mock<DbSet<Accounts>>();
            mockContext.Setup(m => m.Accounts).Returns(mockSet2.Object);

            var deposit = new Deposit(mockContext.Object);

            var Account = new Accounts();
            Account.AccountId = 50070;
            Account.Balance = 700;



            deposit.Account = Account;
            deposit.Amount = 400;

            deposit.SaveTransaction();

            mockSet.Verify(m => m.Add(It.IsAny<Transactions>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
            mockSet.Verify(m => m.Add(It.Is<Transactions>(r => r.Amount == 400m && r.AccountId == 50070 && r.Balance == 1100 && r.Type == "Credit")));
            Assert.IsTrue(Account.Balance == 1100);

        }
    }
}
