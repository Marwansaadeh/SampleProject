using System;
using System.Collections.Generic;

namespace MoneyLaundering
{
    public interface IBatchService
    {
        IEnumerable<SuspectPerson> GetSuspectedPeople(int amountperday, int amounerforcouplesofday, DateTime startdate, DateTime finishdate);
    }
}
