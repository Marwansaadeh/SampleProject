using BankAppData.Models;
using System.Collections.Generic;

namespace BankWebbApp.ViewModels
{
    public class AccountViewModel
    {
        public int AccountId { get; set; }


        public decimal Balance { get; set; }
        public IEnumerable<Transactions> Transactions { get; set; }
        public UserAccountTransctions UserAccountTransctions { get; set; } = new UserAccountTransctions();

    }
    public class UserAccountTransctions
    {

        public int CurrentLoadNumber { get; set; }

        public int CustomerTransctionTotalNumber { get; set; }
        public bool ShowMore()
        {

            if (CurrentLoadNumber > CustomerTransctionTotalNumber + 20)
                return false;
            else return true;

        }

    }
}
