using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BankWebbApp.ViewModels
{
    public class ApplicationUsersDetails
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public List<IdentityRole> AllRoles = new List<IdentityRole>();
    }
}
