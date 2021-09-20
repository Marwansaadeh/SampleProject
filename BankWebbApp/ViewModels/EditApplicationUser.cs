using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankWebbApp.ViewModels
{
    public class EditApplicationUser
    {
        public string Id { get; set; }
        public string SucessMessage { get; set; }
        [Required]
        [EmailAddress]
        [Remote(action: "VerifyEditApplicationUserEmail", controller: "Admin", AdditionalFields = "Email,Id")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^\da-zA-Z])(.{8,15})$", ErrorMessage = "Passord must conatins at least number,upper case character,special character(#.!) and length 8 characters")]

        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]

        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^\da-zA-Z])(.{8,15})$")]
        public string ConfirmPassword { get; set; }
        [Required]
        [EnsureOneElement(ErrorMessage = "One role is required")]
        public List<Roles> AllRoles { get; set; }
    }

}
