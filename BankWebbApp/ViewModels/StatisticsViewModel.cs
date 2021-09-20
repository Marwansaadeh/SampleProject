using System.Collections.Generic;

namespace BankWebbApp.ViewModels
{
    public class StatisticsViewModel
    {
        public int NumberOfCustomers { get; set; }
        public int NumberOfAccounts { get; set; }
        public decimal SumofAccountsBalances { get; set; }
        public List<CountryStatisticsViewModel> countryStatisticsViewModels { get; set; }

    }
}
