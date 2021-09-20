using BankAppData.Models;
using BankWebbApp.AccountTransactions;
using BankWebbApp.Service;
using BankWebbApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace BankWebbApp.Controllers
{
    [Authorize(Roles = "Admin,Cashier")]
    public class AccountController : Controller
    {
        private readonly ITransctionsRepository _TransctionsRepository;
        private readonly IAccountRepository _AccountRepository;
        private readonly BankAppDataContext _context;

        public AccountController(BankAppDataContext context, IAccountRepository accountRepository, ITransctionsRepository transctionsRepository)
        {
            _TransctionsRepository = transctionsRepository;
            _AccountRepository = accountRepository;
            _context = context;

        }
        [HttpPost]
        public ActionResult LoadMore(int currentnumber, int id)
        {
            AccountViewModel model = new AccountViewModel();

            model.UserAccountTransctions.CurrentLoadNumber = currentnumber;
            var Transactions = _TransctionsRepository.GetAccountTransctionsByOrder(id);
            model.UserAccountTransctions.CustomerTransctionTotalNumber = Transactions.Count();

            model.AccountId = id;

            if (currentnumber != 40)
            {
                model.Transactions = Transactions.ToList().SkipLast(model.UserAccountTransctions.CustomerTransctionTotalNumber - currentnumber);

            }
            else
            {
                model.Transactions = Transactions.Take(40);
            }
            return PartialView("_TransctionPartial", model);
        }

        public IActionResult AccountDetails(int id)
        {
            AccountViewModel model = new AccountViewModel();
            model.UserAccountTransctions.CurrentLoadNumber = 20;

            var account = _AccountRepository.GetAccountByID(id);
            if (account != null)
            {
                var Transactions = _TransctionsRepository.GetAccountTransctionsByOrder(id);
                model.UserAccountTransctions.CustomerTransctionTotalNumber = Transactions.Count();
                model.AccountId = account.AccountId;
                model.Balance = account.Balance;
                model.Transactions = Transactions.Take(20);
                return View(model);
            }
            else return View("ResultNotFound");
        }
        [HttpGet]
        public IActionResult Deposit(int id)
        {
            var model = new DepositViewModel();
            model.AccountId = id;

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Deposit(DepositViewModel model)
        {
            var Account = _AccountRepository.GetAccountByID(model.AccountId);
            if (ModelState.IsValid)
            {
                Deposit deposit = new Deposit(_context);
                deposit.Account = Account;
                deposit.Amount = model.Amount;
                deposit.Symbol = model.Symbol;
                deposit.SaveTransaction();
                model.Confirmation = "Deposit transaction has been done successfuly";

            }
            return View(model);
        }

        public IActionResult Withdraw(int id)
        {
            var model = new TransactionViewModel();
            model.AccountId = id;

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Withdraw(TransactionViewModel model)
        {

            var Account = _AccountRepository.GetAccountByID(model.AccountId);
            if (ModelState.IsValid)
            {
                Withdraw withdraw = new Withdraw(_context);
                withdraw.Account = Account;
                withdraw.Amount = model.Amount;
                withdraw.Symbol = model.Symbol;
                withdraw.SaveTransaction();
                model.Confirmation = "Withdraw transaction has been done successfuly";

            }
            return View(model);
        }
        public IActionResult Transfer(int id)
        {
            var model = new TransferTransactionViewModel();
            model.AccountId = id;

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Transfer(TransferTransactionViewModel model)
        {
            var Account = _AccountRepository.GetAccountByID(model.AccountId);
            var RecieverAccount = _AccountRepository.GetAccountByID(model.ReceivedAccount);
            if (ModelState.IsValid)
            {
                Transfer transfer = new Transfer(_context);
                transfer.Account = Account;
                transfer.Amount = model.Amount;
                transfer.Symbol = model.Symbol;
                transfer.Bank = model.Bank;
                transfer.ReceivedAccount = RecieverAccount;
                transfer.SaveTransaction();
                model.Confirmation = "Transfer transaction has been done successfuly";

            }
            return View(model);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyAmount(decimal Amount, int AccountId)
        {
            var Account = _AccountRepository.GetAccountByID(AccountId);

            if (Amount > Account.Balance)
            {
                return Json("You can't transer more amount that exist in your account");
            }

            return Json(true);
        }
        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyDepositAmount(decimal Amount, int AccountId)
        {
            var Account = _AccountRepository.GetAccountByID(AccountId);

            if (Amount <= 0)
            {
                return Json("Please add a valid deposit amount");
            }

            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyReceivedAccount(int ReceivedAccount, int AccountId)
        {
            var receivedaccount = _AccountRepository.GetAccountByID(ReceivedAccount);
            if (ReceivedAccount == AccountId)
            {

                return Json("You can't transer to the same account");
            }
            else if (receivedaccount == null)
            {

                return Json("Recievedaccount can't be found, please enter a valid account number");
            }
            return Json(true);
        }

    }
}
