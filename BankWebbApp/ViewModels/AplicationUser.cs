using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BankWebbApp.ViewModels
{
    public class AplicationUser
    {
        public string SucessMessage { get; set; }

        [Required]
        [EmailAddress]
        [Remote(action: "VerifyApplicationUserEmail", controller: "Admin", AdditionalFields = nameof(Email))]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^\da-zA-Z])(.{8,15})$", ErrorMessage = "Passord must conatins at least number,upper case character,special character(#.!) and length 8 characters")]

        public string Password { get; set; }

        [Required(ErrorMessage = "Confirmation Password is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]

        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^\da-zA-Z])(.{8,15})$")]
        public string ConfirmPassword { get; set; }


        [EnsureOneElement(ErrorMessage = "one role is required")]
        public List<Roles> AllRoles { get; set; }
    }
    public class Roles
    {
        public bool IsSelected { get; set; }
        public string Id { get; set; }
        public string NormalizedName { get; set; }
        public string Name { get; set; }

    }
    public class EnsureOneElementAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var list = value as List<Roles>;
            if (list != null)
            {
                return list.Where(x => x.IsSelected == true).Count() == 1;
            }
            return false;
        }
    }
}
