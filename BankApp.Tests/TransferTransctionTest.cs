using BankAppData.Models;
using BankWebbApp.AccountTransactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
namespace BankApp.Tests
{
    [TestClass]
    public class TransferTransctionTest
    {
        [TestMethod]
        public void TransferClass_SaveCorrectTransctionAndUpateTwoAccountsBlancesAfterTransction()
        {


            var mockSet = new Mock<DbSet<Transactions>>();
            var mockContext = new Mock<BankAppDataContext>();
            mockContext.Setup(m => m.Transactions).Returns(mockSet.Object);

            var mockSet2 = new Mock<DbSet<Accounts>>();
            mockContext.Setup(m => m.Accounts).Returns(mockSet2.Object);

            var transfer = new Transfer(mockContext.Object);

            var SendedAccount = new Accounts();
            SendedAccount.AccountId = 450;
            SendedAccount.Balance = 700;

            var ReceivedAccount = new Accounts();
            ReceivedAccount.AccountId = 460;
            ReceivedAccount.Balance = 500;

            transfer.Account = SendedAccount;
            transfer.ReceivedAccount = ReceivedAccount;
            transfer.Amount = 400;
            mockSet.Verify(m => m.Add(It.IsAny<Transactions>()), Times.Exactly(0));

            transfer.SaveTransaction();
            mockSet.Verify(m => m.Add(It.IsAny<Transactions>()), Times.Exactly(2));


            mockContext.Verify(m => m.SaveChanges(), Times.Once());

            mockSet.Verify(m => m.Add(It.Is<Transactions>(r => r.Amount == -400m && r.AccountId == 450 && r.Balance == 300 && r.Type == "Debit")));
            mockSet.Verify(m => m.Add(It.Is<Transactions>(r => r.Amount == 400m && r.AccountId == 460 && r.Balance == 900 && r.Type == "Credit")));

            Assert.IsTrue(ReceivedAccount.Balance == 900);
            Assert.IsTrue(SendedAccount.Balance == 300);
        }
    }
}
