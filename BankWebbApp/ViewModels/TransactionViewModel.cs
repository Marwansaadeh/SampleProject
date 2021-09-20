using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace BankWebbApp.ViewModels
{
    public class TransactionViewModel
    {
        public int TransactionId { get; set; }
        [Required]

        public int AccountId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter valid amount")]
        [Remote(action: "VerifyAmount", controller: "Account", AdditionalFields = nameof(AccountId))]
        public decimal Amount { get; set; }



        public string Symbol { get; set; }
        public string Confirmation { get; set; }
    }
}
