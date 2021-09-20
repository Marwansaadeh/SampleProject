using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace BankWebbApp.ViewModels
{
    public class CustomerViewModel
    {
        public string SucessMessage { get; set; }

        public int CustomerId { get; set; }

        [BindProperty]
        [StringLength(6)]
        public string Gender { get; set; }
        public string[] Genders = new[] { "Male", "Female" };

        [Required]
        [StringLength(100)]

        public string Givenname { get; set; }
        [Required]
        [StringLength(100)]

        public string Surname { get; set; }
        [Required]
        [StringLength(100)]

        public string Streetaddress { get; set; }
        [Required]
        [StringLength(100)]

        public string City { get; set; }
        [Required]
        [StringLength(15)]

        public string Zipcode { get; set; }
        [Required]
        [StringLength(100)]
        public string Country { get; set; }
        [Required]

        [StringLength(2, ErrorMessage = "Please enter a valid Country Code max 2 character")]
        public string CountryCode { get; set; }
        [DataType(DataType.Date)]
        [DataBirthValidation(ErrorMessage = "Please enter a valid birth date")]

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? Birthday { get; set; }
        [StringLength(10)]
        public string NationalId { get; set; }
        [StringLength(25)]
        public string Telephonecountrycode { get; set; }
        [StringLength(100)]
        public string Telephonenumber { get; set; }
        public string Emailaddress { get; set; }

    }

    public class DataBirthValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                DateTime date;
                DateTime.TryParse(value.ToString(), out date);
                if (date < DateTime.Now && (DateTime.Now.Year - date.Year <= 120))
                {
                    return true;
                }
                else return false;
            }
            return true;
        }
    }
}
