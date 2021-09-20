using BankAppData.Models;
using BankWebbApp.Service;
using System.Collections.Generic;

namespace MoneyLaundering.Service
{
    public class SuspectPeople : ISuspectPeople
    {
        private readonly ICustomerRepository _customerRepository;

        public SuspectPeople(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;

        }
        public SuspectPerson AddSuspectPerson(int AccountNumber, IEnumerable<Transactions> Transactions)
        {
            var suspectedPerson = new SuspectPerson();
            var Customer = _customerRepository.FindCustomerByAccountNumber(AccountNumber);

            suspectedPerson.CustomerName = Customer.Givenname;
            suspectedPerson.Country = Customer.Country;
            suspectedPerson.Accountnumber = AccountNumber;
            foreach (var transactions in Transactions)
            {

                Transactions transaction = new Transactions();
                transaction.Account = transactions.Account;
                transaction.AccountId = transactions.AccountId;
                transaction.Date = transactions.Date;
                transaction.Amount = transactions.Amount;
                transaction.Balance = transactions.Balance;
                transaction.TransactionId = transactions.TransactionId;
                transaction.Amount = transactions.Amount;
                suspectedPerson.Transactions.Add(transaction);

            }
            return suspectedPerson;
        }
    }
}
