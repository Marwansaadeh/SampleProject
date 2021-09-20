using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace BankWebbApp.ViewModels
{
    public class TransferTransactionViewModel
    {

        public int TransactionId { get; set; }
        [Required]

        public int AccountId { get; set; }


        [Remote(action: "VerifyAmount", controller: "Account", AdditionalFields = nameof(AccountId))]
        public decimal Amount { get; set; }
        public string Bank { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "Please enter valid Account Number")]
        [Required]
        [Remote(action: "VerifyReceivedAccount", controller: "Account", AdditionalFields = nameof(AccountId))]
        public int ReceivedAccount { get; set; }

        public string Symbol { get; set; }
        public string Confirmation { get; set; }

    }
}
