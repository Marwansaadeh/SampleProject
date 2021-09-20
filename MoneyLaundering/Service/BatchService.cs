using BankWebbApp.Service;
using MoneyLaundering.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoneyLaundering
{
    public class BatchService : IBatchService
    {

        private readonly ITransctionsRepository _transctionsRepository;
        private readonly ISuspectPeople _suspectPeople;


        public BatchService(ITransctionsRepository transctionsRepository, ISuspectPeople suspectPeople)
        {
            _transctionsRepository = transctionsRepository;
            _suspectPeople = suspectPeople;
        }
        public IEnumerable<SuspectPerson> GetSuspectedPeople(int amountperday, int amounerforcouplesofday, DateTime startdate, DateTime finishdate)
        {
            List<SuspectPerson> SuspectedPeopleList = new List<SuspectPerson>();
            var lastThreeDaysTransctions = _transctionsRepository.GetLastTreeDaysTransactions(startdate, finishdate);
            var Accountstransctions = lastThreeDaysTransctions.GroupBy(x => x.AccountId).Select(x => new
            {
                Account = x.Key,
                Tranctions = x
            });

            foreach (var AccountTranction in Accountstransctions)
            {
                var Account = AccountTranction.Account;

                var TranstionsLessThanAmountPerDay = AccountTranction.Tranctions.Where(k => k.AccountId == AccountTranction.Account && k.Amount < amountperday && k.Amount > -amountperday);

                var TranstionsValuesLessThanAmountPerDay = TranstionsLessThanAmountPerDay.Where(x => x.Amount < amountperday && x.Amount > -amountperday).Select(x => x.Amount < 0 ? -x.Amount : x.Amount);

                if (TranstionsValuesLessThanAmountPerDay.Sum() > amounerforcouplesofday)
                {
                    SuspectPerson suspectPerson = _suspectPeople.AddSuspectPerson(Account, TranstionsLessThanAmountPerDay);
                    SuspectedPeopleList.Add(suspectPerson);

                }
                var TranstionsValuesMoreThanAmountPerDay = AccountTranction.Tranctions.Select(x => x.Amount < 0 ? -x.Amount : x.Amount);

                if (TranstionsValuesMoreThanAmountPerDay.Any(x => x > amountperday))
                {
                    var TranstionsMoreThanAmountPerDay = AccountTranction.Tranctions.Where(x => x.Amount > amountperday || x.Amount < -amountperday);
                    SuspectPerson suspectPerson = _suspectPeople.AddSuspectPerson(Account, TranstionsMoreThanAmountPerDay);
                    SuspectedPeopleList.Add(suspectPerson);

                }
            }
            return SuspectedPeopleList;
        }


    }
}
