using System.Collections.Generic;
using System.Linq;

namespace MoneyLaundering
{
    public interface IEmailSender
    {
        void SendEmail(IEnumerable<IGrouping<string, SuspectPerson>> keyValuePairs);
    }
}
