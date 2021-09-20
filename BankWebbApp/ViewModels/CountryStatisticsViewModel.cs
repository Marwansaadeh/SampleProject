using BankAppData.Models;
using System.Collections.Generic;

namespace BankWebbApp.ViewModels
{
    public class CountryStatisticsViewModel
    {
        public StatisticsViewModel statisticsViewModel { get; set; }
        public string CountryName { get; set; }
        public IEnumerable<Customers> HeightTenscutomeraccounts { get; set; }
    }
}
